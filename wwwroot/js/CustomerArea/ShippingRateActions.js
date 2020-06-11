AddEventListenerToCalculateShippingRates();

function AddEventListenerToCalculateShippingRates() {
    const shippingRateButton = document.getElementById('shipping-rate-button');

    if (shippingRateButton == null)
        return;

    shippingRateButton.onclick = () => {
        document.getElementById('proceed-button').classList.add('disabled');
        document.getElementById('proceed-button').classList.add('tool');
        
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
            ShippingRateErrorBehaviour();
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

function ShippingRateErrorBehaviour() {
    document.getElementById('shipping-rate-error').style.display = 'block';
    document.getElementById('shipping-rate-options').style.display = 'none'; 
    ChangeShippingRateValue(0);
}

function SuccessfulCalculationBehaviour(response) {
    const shippingInfos = JSON.parse(response);
    AddEventListenersToRadioInputs(shippingInfos);

    document.getElementById('pac-price').innerHTML = '$' + ConvertNumericPriceToStringPrice(shippingInfos[0].price);
    document.getElementById('sedex-price').innerHTML = '$' + ConvertNumericPriceToStringPrice(shippingInfos[1].price);
    document.getElementById('pac-eta').innerHTML = shippingInfos[0].estimatedTimeOfArrivalInDays + ' days';
    document.getElementById('sedex-eta').innerHTML = shippingInfos[1].estimatedTimeOfArrivalInDays + ' days';

    document.getElementById('shipping-rate-options').style.display = 'block'; 
    document.getElementById('shipping-rate-error').style.display = 'none';
    document.getElementById('proceed-button').classList.remove('disabled');
    document.getElementById('proceed-button').classList.remove('tool');
}

function AddEventListenersToRadioInputs(shippingInfos) {
    const pacRadioInput = document.getElementById('pac-option');
    const sedexRadioInput = document.getElementById('sedex-option');
    const radioInputContainers = document.getElementById('shipping-rate-options').getElementsByTagName('dl');

    pacRadioInput.onchange = () => {
        radioInputContainers[0].classList.add('selected-radio-input');
        radioInputContainers[1].classList.remove('selected-radio-input');

        ChangeSelectedShippingRate(shippingInfos[0]);

        shippingInfos.forEach(element => element.isSelected = false);
        shippingInfos[0].isSelected = true;
    };
    sedexRadioInput.onchange = () => {
        radioInputContainers[0].classList.remove('selected-radio-input');
        radioInputContainers[1].classList.add('selected-radio-input');

        ChangeSelectedShippingRate(shippingInfos[1]);

        shippingInfos.forEach(element => element.isSelected = false);
        shippingInfos[1].isSelected = true;
    };

    if (shippingInfos[0].isSelected === true) {
        pacRadioInput.checked = true;
        pacRadioInput.onchange();
    }
    else if (shippingInfos[1].isSelected === true) {
        sedexRadioInput.checked = true;
        sedexRadioInput.onchange();
    }
}

function ChangeSelectedShippingRate(shippingInfo) {
    if (shippingInfo.isSelected === true) {
        ChangeShippingRateValue(shippingInfo.price);
        return;
    }
    
    const xmlHttpRequest = new XMLHttpRequest();

    xmlHttpRequest.addEventListener('load', (response) => {
        const isAnErrorResponse = response.originalTarget.status != 200;
        if (isAnErrorResponse) {
            ShippingRateErrorBehaviour();
        }
        else {
            ChangeShippingRateValue(shippingInfo.price);
        }
    }, false);

    const url = `/Cart/ChangeSelectedShippingRate?freightType=${shippingInfo.freightType}`;
    xmlHttpRequest.open('get', url, true);
    xmlHttpRequest.send();
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