AddEventListenerToCalculateShippingRates();

function AddEventListenerToCalculateShippingRates() {
    const shippingRateButton = document.getElementById('shipping-rate-button');

    if (shippingRateButton == null)
        return;

    shippingRateButton.onclick = () => {
        document.getElementById('proceed-button').classList.add('disabled');
        
        let destinationCep = document.getElementById('cep-input').value;
        destinationCep = destinationCep.replace('.', '').replace('-', '');

        if (destinationCep.length != 8)
            document.getElementById('cep-validation').style.display= 'block';
        else
        {
            document.getElementById('cep-validation').style.display = 'none';
            CalculateShippingRates(destinationCep);
        }
    };

    /* Calls the calculation functions if the CEP input field is filled (with cookie data). */
    shippingRateButton.onclick();
    document.getElementById('cep-validation').style.display = 'none';
}

function CalculateShippingRates(destinationCep) {
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

    const url = `/Cart/CalculateShippingRate?destinationCep=${destinationCep}`;
    xmlHttpRequest.open('get', url, true);
    xmlHttpRequest.send();

    document.getElementById('loading-animation').style.display = 'block';
}

function CalculationErrorBehaviour() {
    document.getElementById('shipping-rate-error').style.display = 'block';
    document.getElementById('shipping-rate-options').style.display = 'none'; 
    ChangeShippingRateValue(0);
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
    document.getElementById('proceed-button').classList.remove('disabled');
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