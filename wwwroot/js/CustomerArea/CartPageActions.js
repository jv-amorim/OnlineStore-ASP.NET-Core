AddEventListenersToChangeProductAmountInCart();     /* JS Hoisting. */

function AddEventListenersToChangeProductAmountInCart() {
    const increaseButtons = document.getElementsByClassName('increase-button');
    const decreaseButtons = document.getElementsByClassName('decrease-button');
    
    for (let index = 0; index < increaseButtons.length; index++)
    {
        increaseButtons[index].addEventListener("click", () => {
            const productAmountManager = new ProductAmountManager(index);
            productAmountManager.ChangeProductAmountInCart('+');
        });

        decreaseButtons[index].addEventListener("click", () => {
            const productAmountManager = new ProductAmountManager(index);
            productAmountManager.ChangeProductAmountInCart('-');
        });
    }
}

class ProductAmountManager {
    constructor(productIndexInCart) {
        this.productIndexInCart = productIndexInCart;
        this.productIdValue = document.getElementsByClassName('product-id')[productIndexInCart].value;
        this.amount = document.getElementsByClassName('product-amount')[productIndexInCart];
        this.unitsInStock = document.getElementsByClassName('product-unitsInStock')[productIndexInCart];
        this.unitPrice = document.getElementsByClassName('product-unitPrice')[productIndexInCart];
        this.subtotalPrice = document.getElementsByClassName('product-subtotal')[productIndexInCart];

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
        this.SetSubtotalPrice();
        this.ChangeProductAmountInTheCartCookie();
    }

    ValidateAmountValue() {
        if (this.currentAmountValue > parseInt(this.unitsInStock.value)) {
            this.ShowUnitsInStockMessage();
            return false;
        }
        if (this.currentAmountValue < 1) {
            this.RemoveProductFromCart();
            return false;
        }
        return true;
    }

    SetSubtotalPrice() {
        const newSubtotal = 
            (this.currentAmountValue * parseFloat(this.unitPrice.value))
            .toLocaleString(undefined, {
                minimumFractionDigits: 2, 
                maximumFractionDigits: 2, 
                currency: 'USD'
            });
        this.subtotalPrice.innerHTML = '$' + newSubtotal;
    }

    ShowUnitsInStockMessage() {
        const unitsInStockMessage = document.getElementById('unitsInStockMessage');
        unitsInStockMessage.style.display = 'block';
        setTimeout(() => unitsInStockMessage.style.display = 'none', 5000);
    }

    ShowRemovalErrorMessage() {
        const unitsInStockMessage = document.getElementById('removalErrorMessage');
        unitsInStockMessage.style.display = 'block';
        setTimeout(() => unitsInStockMessage.style.display = 'none', 5000);
    }

    RemoveProductFromCart() {
        const productRow = document.getElementsByClassName('product-row')[this.productIndexInCart];
        productRow.style.display = 'none';
        
        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.addEventListener('error', () => {
            this.ShowRemovalErrorMessage();
            this.amount.value = this.originalAmountValue;
            this.currentAmountValue = this.originalAmountValue;
            this.SetSubtotalPrice();
            productRow.style.display = 'contents';
        }, false);

        const url = `/Cart/RemoveProductFromCart?id=${this.productIdValue}`;
        xmlHttpRequest.open('get', url, true);
        xmlHttpRequest.send();
    }
    
    ChangeProductAmountInTheCartCookie() {
        const xmlHttpRequest = new XMLHttpRequest();

        xmlHttpRequest.addEventListener('error', () => {
            this.amount.value = this.originalAmountValue;
            this.currentAmountValue = this.originalAmountValue;
            this.SetSubtotalPrice();
        }, false);

        const url = `/Cart/UpdateProductInCart?id=${this.productIdValue}&amount=${this.currentAmountValue}`;
        xmlHttpRequest.open('get', url, true);
        xmlHttpRequest.send();
    }
}