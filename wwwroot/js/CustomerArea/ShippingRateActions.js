AddEventListenerToCalculateShippingRates();

function AddEventListenerToCalculateShippingRates() {
    const shippingRateButton = document.getElementById('shipping-rate-button');

    if (shippingRateButton == null)
        return;

    shippingRateButton.addEventListener('click', () => {
        let destinationCEP = document.getElementById('shipping-rate-input').value;
        destinationCEP = destinationCEP.replace('.', '').replace('-', '');
        CalculateShippingRates(destinationCEP);
    });
}

function CalculateShippingRates(destinationCEP) {
    const xmlHttpRequest = new XMLHttpRequest();

    xmlHttpRequest.addEventListener('load', (response) => {
        const isAnErrorResponse = response.originalTarget.status != 200;
        if (isAnErrorResponse) {
            document.getElementById('shipping-rate-error').style.display = 'block';
            document.getElementById('shipping-rate-options').style.display = 'none'; 
        }
        else {
            document.getElementById('shipping-rate-options').style.display = 'block'; 
            document.getElementById('shipping-rate-error').style.display = 'none';
            // TODO - Show shipping rates and ETA in the view.
            const shippingInfo = JSON.parse(response.originalTarget.response);
            console.log(shippingInfo);
        }
    }, false);

    const url = `/Cart/CalculateShippingRate?destinationCEP=${destinationCEP}`;
    xmlHttpRequest.open('get', url, true);
    xmlHttpRequest.send();
}