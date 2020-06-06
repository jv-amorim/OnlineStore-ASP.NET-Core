AddEventListenerToCalculateShippingRates();

function AddEventListenerToCalculateShippingRates() {
    const shippingRateButton = document.getElementById('shipping-rate-button');

    if (shippingRateButton == null)
        return;

    shippingRateButton.addEventListener('click', () => {
        let destinationCEP = document.getElementById('cep-input').value;
        destinationCEP = destinationCEP.replace('.', '').replace('-', '');
        CalculateShippingRates(destinationCEP);
    });
}

function CalculateShippingRates(destinationCEP) {
    const xmlHttpRequest = new XMLHttpRequest();

    xmlHttpRequest.addEventListener('load', (response) => {
        const isAnErrorResponse = response.originalTarget.status != 200;
        if (isAnErrorResponse) {
            CalculationErrorBehaviour();
        }
        else {       
            SuccessfulCalculationBehaviour(response.originalTarget.response);
        }
        document.getElementById('loading-animation').style.display = 'none';
    }, false);

    const url = `/Cart/CalculateShippingRate?destinationCEP=${destinationCEP}`;
    xmlHttpRequest.open('get', url, true);
    xmlHttpRequest.send();

    document.getElementById('loading-animation').style.display = 'block';
}

function CalculationErrorBehaviour() {
    document.getElementById('shipping-rate-error').style.display = 'block';
    document.getElementById('shipping-rate-options').style.display = 'none'; 
}

function SuccessfulCalculationBehaviour(response) {
    const shippingInfo = JSON.parse(response);
    AddEventListenersToRadioInputs(shippingInfo);

    document.getElementById('pac-price').innerHTML = '$' + ConvertNumericPriceToStringPrice(shippingInfo[0].price);
    document.getElementById('sedex-price').innerHTML = '$' + ConvertNumericPriceToStringPrice(shippingInfo[1].price);
    document.getElementById('pac-eta').innerHTML = shippingInfo[0].estimatedTimeOfArrivalInDays + ' days';
    document.getElementById('sedex-eta').innerHTML = shippingInfo[1].estimatedTimeOfArrivalInDays + ' days';

    document.getElementById('shipping-rate-options').style.display = 'block'; 
    document.getElementById('shipping-rate-error').style.display = 'none';
}

function AddEventListenersToRadioInputs(shippingInfo) {
    const pacRadioInput = document.getElementById('pac-option');
    const sedexRadioInput = document.getElementById('sedex-option');
    const radioInputContainers = document.getElementById('shipping-rate-options').getElementsByTagName('dl');

    pacRadioInput.onchange = () => {
        radioInputContainers[0].classList.add('selected-radio-input');
        radioInputContainers[1].classList.remove('selected-radio-input');
        ChangeShippingRateValue(shippingInfo[0].price);
    };
    sedexRadioInput.onchange = () => {
        radioInputContainers[0].classList.remove('selected-radio-input');
        radioInputContainers[1].classList.add('selected-radio-input');
        ChangeShippingRateValue(shippingInfo[1].price);
    };

    pacRadioInput.checked = true;
    pacRadioInput.onchange();
}

function ChangeShippingRateValue(value) {
    const cartShippingRateText = document.getElementById('cart-shipping-rate-text');
    const cartShippingRateInput = document.getElementById('cart-shipping-rate-input');
    cartShippingRateText.innerHTML = '$' + ConvertNumericPriceToStringPrice(value);
    cartShippingRateInput.value = value;
    cartShippingRateInput.onchange();
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