function AddEventListenersToChangeProductAmountInCart() {
    const increaseButtons = document.getElementsByClassName('increase-button');
    const decreaseButtons = document.getElementsByClassName('decrease-button');
    
    for (let index = 0; index < increaseButtons.length; index++)
        increaseButtons[index].addEventListener("click", () => IncreaseProductAmountInCart(index));

    for (let index = 0; index < decreaseButtons.length; index++)
        decreaseButtons[index].addEventListener("click", () => DecreaseProductAmountInCart(index));
}

function IncreaseProductAmountInCart(index) {
    const productId = document.getElementsByClassName('product-id')[index].value;
    const amount = document.getElementsByClassName('product-amount')[index].value;
    const unitsInStock = document.getElementsByClassName('product-unitsInStock')[index].value;
    const unitPrice = document.getElementsByClassName('product-unitPrice')[index].value;

    alert(`${productId} | ${amount} | ${unitsInStock} | ${unitPrice}`);
}

function DecreaseProductAmountInCart(index) {
    const productId = document.getElementsByClassName('product-id')[index].value;
    const amount = document.getElementsByClassName('product-amount')[index].value;
    const unitsInStock = document.getElementsByClassName('product-unitsInStock')[index].value;
    const unitPrice = document.getElementsByClassName('product-unitPrice')[index].value;

    alert(`${productId} | ${amount} | ${unitsInStock} | ${unitPrice}`);
}

AddEventListenersToChangeProductAmountInCart();