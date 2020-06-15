AddEventListenerToTheSaveAddressButton();

function AddEventListenerToTheSaveAddressButton() {
    const saveAddressButton = document.getElementById('save-address-button');

    if (saveAddressButton == null)
        return;

    saveAddressButton.onclick = () => {
        if (ValidateAddressFields()) {
            saveAddressButton.classList.add('disabled');
            saveAddressButton.innerHTML = 'Saved <i class="fas fa-check"></i>';
            document.getElementById('address-save-status').value = 'true';
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