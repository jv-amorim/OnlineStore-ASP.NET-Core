AddEventListenersToChangeProductAmountInCart();     /* JS Hoisting. */
AddEventListenerToChangeCartShippingRate();

function AddEventListenersToChangeProductAmountInCart() {
    const increaseButtons = document.getElementsByClassName('increase-button');
    const decreaseButtons = document.getElementsByClassName('decrease-button');

    for (let index = 0; index < increaseButtons.length; index++)
    {
        increaseButtons[index].addEventListener("click", () => {
            const productAmountManager = new ProductAmountManager(index);
            const cartTotalPriceManager = new CartTotalPriceManager();
            productAmountManager.ChangeProductAmountInCart('+');
            cartTotalPriceManager.UpdateCartPrices();
        });

        decreaseButtons[index].addEventListener("click", () => {
            const productAmountManager = new ProductAmountManager(index);
            const cartTotalPriceManager = new CartTotalPriceManager();
            productAmountManager.ChangeProductAmountInCart('-');
            cartTotalPriceManager.UpdateCartPrices();
        });
    }
}

function AddEventListenerToChangeCartShippingRate() {
    const cartShippingRateInput = document.getElementById('cart-shipping-rate-input');

    if (cartShippingRateInput == null)
        return;
        
    cartShippingRateInput.onchange = () => {
        const cartTotalPriceManager = new CartTotalPriceManager();
        cartTotalPriceManager.UpdateCartPrices();
    }
}

class ProductAmountManager {
    constructor(productIndexInCart) {
        this.productIndexInCart = productIndexInCart;
        this.productIdValue = document.getElementsByClassName('product-id')[productIndexInCart].value;
        this.amount = document.getElementsByClassName('product-amount')[productIndexInCart];
        this.unitsInStock = document.getElementsByClassName('product-units-in-stock')[productIndexInCart];
        this.unitPrice = document.getElementsByClassName('product-unit-price')[productIndexInCart];
        this.productTotalPrice = document.getElementsByClassName('product-total-price')[productIndexInCart];

        this.originalAmountValue = parseInt(this.amount.value);
        this.currentAmountValue = parseInt(this.amount.value);
    }

    ChangeProductAmountInCart(operation) {
        document.getElementById('proceed-button').classList.add('disabled');
        document.getElementById('proceed-button').classList.add('tool');
        
        if (operation === '+')
            this.currentAmountValue++;
        else if (operation === '-')
            this.currentAmountValue--;

        if (this.ValidateAmountValue() == false)
            return;

        this.amount.value = this.currentAmountValue;
        this.SetProductTotalPrice();
        this.ChangeProductAmountInTheCartCookie();
    }

    ValidateAmountValue() {
        if (this.currentAmountValue > parseInt(this.unitsInStock.value)) {
            this.ShowCartErrorMessage('Sorry, there are no more units of this product in stock.');
            return false;
        }
        if (this.currentAmountValue < 1) {
            this.SetProductTotalPrice();
            this.RemoveProductFromCart();
            return false;
        }
        return true;
    }

    SetProductTotalPrice() {
        const newTotalPrice = this.currentAmountValue * parseFloat(this.unitPrice.value);
        this.productTotalPrice.setAttribute('value', newTotalPrice);
        this.productTotalPrice.innerHTML = '$' + ConvertNumericPriceToStringPrice(newTotalPrice);
    }

    ShowCartErrorMessage(message) {
        const cartErrorMessage = document.getElementById('cart-error-message');
        cartErrorMessage.innerHTML = message;
        cartErrorMessage.style.display = 'block';
        setTimeout(() => cartErrorMessage.style.display = 'none', 5000);
    }
    
    ChangeProductAmountInTheCartCookie() {
        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.addEventListener('error', () => {
            this.amount.value = this.originalAmountValue;
            this.currentAmountValue = this.originalAmountValue;
            this.SetProductTotalPrice();
        }, false);

        const url = `/Cart/UpdateProductInCart?id=${this.productIdValue}&amount=${this.currentAmountValue}`;
        xmlHttpRequest.open('get', url, true);
        xmlHttpRequest.send();
    }

    RemoveProductFromCart() {
        const productRow = document.getElementsByClassName('product-row')[this.productIndexInCart];
        productRow.style.display = 'none';
        
        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.addEventListener('error', () => {
            this.amount.value = this.originalAmountValue;
            this.currentAmountValue = this.originalAmountValue;
            this.SetProductTotalPrice();
            this.ShowCartErrorMessage('Sorry, the product has not been removed, please try again.');
            productRow.style.display = 'contents';
        }, false);

        const url = `/Cart/RemoveProductFromCart?id=${this.productIdValue}`;
        xmlHttpRequest.open('get', url, true);
        xmlHttpRequest.send();
    }
}

class CartTotalPriceManager {
    constructor() {
        this.productTotalPrices = document.getElementsByClassName('product-total-price');
        this.cartSubtotalPrice = document.getElementById('cart-subtotal-price');
        this.cartShippingRateInput = document.getElementById('cart-shipping-rate-input');
        this.cartTotalPrice = document.getElementById('cart-total-price');

        this.cartSubtotalPriceValue = 0;
    }

    UpdateCartPrices() {
        this.UpdateSubtotalPrice();
        this.UpdateTotalPrice();
    }

    UpdateSubtotalPrice() {
        for (let i = 0; i < this.productTotalPrices.length; i++) {
            const priceValue = parseFloat(this.productTotalPrices[i].getAttribute('value'));
            this.cartSubtotalPriceValue += priceValue;
        }
        const subtotalprice = ConvertNumericPriceToStringPrice(this.cartSubtotalPriceValue);
        this.cartSubtotalPrice.innerHTML = '$' + subtotalprice;
    }

    UpdateTotalPrice() {
        const cartShippingRateValue = parseFloat(this.cartShippingRateInput.value);
        const totalPrice = 
            ConvertNumericPriceToStringPrice(this.cartSubtotalPriceValue + cartShippingRateValue);
        this.cartTotalPrice.innerHTML = `<b>$${totalPrice}</b>`;
    }
}

function ConvertNumericPriceToStringPrice(numericPrice) {
    return (
        numericPrice
        .toLocaleString(undefined, {
            minimumFractionDigits: 2, 
            maximumFractionDigits: 2, 
            currency: 'USD'
        })
    );
}