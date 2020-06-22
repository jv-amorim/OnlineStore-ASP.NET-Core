function ApplyMasksToInputFields() {
    $('#cep-input').mask('00.000-000');
    $('.cep-input').mask('00.000-000');
    $('.cpf-input-field').mask('000.000.000-00');
    $('.phone-input-field').mask('(00) 00000-0000');
    $('.month-year').mask('00/00');
}

ApplyMasksToInputFields();