AddEventListenerToTheSaveAddressButton();
AddEventListenerToTheCepInputField();

function AddEventListenerToTheSaveAddressButton() {
    const saveAddressButton = document.getElementById('save-address-button');
    const addressSaveStatus = document.getElementById('address-save-status');

    if (addressSaveStatus == null)
        return;

    saveAddressButton.onclick = () => {
        if (ValidateAddressFields()) {
            saveAddressButton.classList.add('disabled');
            saveAddressButton.innerHTML = 'Saved <i class="fas fa-check"></i>';
            addressSaveStatus.value = 'true';
            DisableAddressFields();
        }
        else {
            document.getElementById('address-validation-error').style.display = 'block';
        }
    };
}

function ValidateAddressFields() {
    const cepFieldValue = 
        document.getElementById('cep').value
        .replace('.', '').replace('-', '');
    
    if (cepFieldValue == '' || cepFieldValue.length < 8 ) 
        return false;
    if (document.getElementById('city').value == '') 
        return false;
    if (document.getElementById('state').value == '') 
        return false;
    if (document.getElementById('address-line').value == '') 
        return false;
    if (document.getElementById('neighborhood').value == '') 
        return false;
    if (document.getElementById('number').value == '') 
        return false;

    return true;
}

function DisableAddressFields() {
    const cepField = document.getElementById('cep');
    cepField.value = cepField.value.replace('.', '').replace('-', '');
    cepField.setAttribute('readonly', 'true');
    
    document.getElementById('city').setAttribute('readonly', 'true');
    document.getElementById('state').setAttribute('readonly', 'true');
    document.getElementById('address-line').setAttribute('readonly', 'true');
    document.getElementById('neighborhood').setAttribute('readonly', 'true');
    document.getElementById('number').setAttribute('readonly', 'true');
    document.getElementById('complement').setAttribute('readonly', 'true');
    document.getElementById('address-validation-error').style.display = 'none';
}

function AddEventListenerToTheCepInputField() {
    const cepField = document.getElementById('cep');

    if (cepField == null)
        return;

    cepField.onkeyup = () => {
        const cepFieldValue = cepField.value.replace('.', '').replace('-', '');

        if (cepFieldValue.length < 8)
            return;

        GetAddressDataFromCep(cepFieldValue);
    };
}

function GetAddressDataFromCep(cep) {
    const xmlHttpRequest = new XMLHttpRequest();

    xmlHttpRequest.addEventListener('load', (response) => {
        const isSuccessfulResponse = response.originalTarget.status == 200;
        if (isSuccessfulResponse) {
            const responseData = JSON.parse(response.originalTarget.response);

            if (responseData.erro == true) {
                ClearAddressFields();
                return;
            }

            SetAddressFieldsWithDataFromResponse(responseData);
        }
    }, false);

    const url = `https://viacep.com.br/ws/${cep}/json/`;
    xmlHttpRequest.open('get', url, true);
    xmlHttpRequest.send();
}

function ClearAddressFields() {
    document.getElementById('city').value = "";
    document.getElementById('state').value = "";
    document.getElementById('address-line').value = "";
    document.getElementById('neighborhood').value = "";
    document.getElementById('complement').value = "";
}

function SetAddressFieldsWithDataFromResponse(data) {
    ClearAddressFields();
    document.getElementById('city').value = data.localidade;
    document.getElementById('state').value = data.uf;
    document.getElementById('address-line').value = data.logradouro;
    document.getElementById('neighborhood').value = data.bairro;
    document.getElementById('complement').value = data.complemento;
}