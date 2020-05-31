AddEventListenersToChangeProductAmountInCart();     /* JS Hoisting. */

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

class ProductAmountManager {
    constructor(productIndexInCart) {
        this.productIndexInCart = productIndexInCart;
        this.productIdValue = document.getElementsByClassName('product-id')[productIndexInCart].value;
        this.amount = document.getElementsByClassName('product-amount')[productIndexInCart];
        this.unitsInStock = document.getElementsByClassName('product-unitsInStock')[productIndexInCart];
        this.unitPrice = document.getElementsByClassName('product-unitPrice')[productIndexInCart];
        this.productTotalPrice = document.getElementsByClassName('product-totalPrice')[productIndexInCart];

        this.originalAmountValue = parseInt(this.amount.value);
        this.currentAmountValue = parseInt(this.amount.value);
    }

    ChangeProductAmountInCart(operation) {
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
        this.productTotalPrices = document.getElementsByClassName('product-totalPrice');
        this.cartSubtotalPrice = document.getElementById('cart-subtotalPrice');
        this.cartShippingRate = document.getElementById('cart-shippingRate');
        this.cartTotalPrice = document.getElementById('cart-totalPrice');

        this.cartSubtotalPriceValue = 0;
        this.cartShippingRateValue = 0;
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
        const totalPrice = 
            ConvertNumericPriceToStringPrice(this.cartSubtotalPriceValue + this.cartShippingRateValue);
        this.cartTotalPrice.innerHTML = `<b>$${totalPrice}</b>`;
    }
}