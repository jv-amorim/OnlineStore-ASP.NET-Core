function AddConfirmationAlerts() {
    const dangerButtons = document.getElementsByClassName('btn-danger');

    if (dangerButtons == null)
        return;
    if (dangerButtons.length == 0)
        return;

    Array.from(dangerButtons)
    .forEach(button => button.addEventListener('click', (e) => {
        const result = confirm("Do you really want to do this action?");
        if (!result)
            e.preventDefault();
    }));
}

AddConfirmationAlerts();