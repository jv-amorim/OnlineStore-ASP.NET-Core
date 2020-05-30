function AddEventListenerToChangeProductSorting() {
    const sortingDropdown = document.getElementById('sortingOption');
   
    if (sortingDropdown == null)
        return;

    sortingDropdown.addEventListener('change', () => {
        let page = 1;
        let searchParameter = "";
        const sortingOption = sortingDropdown.value;

        const queryString = new URLSearchParams(window.location.search);

        if (queryString.has("page"))
            page = queryString.get("page");
        if (queryString.has("searchParameter"))
            searchParameter = queryString.get("searchParameter");

        var uri = 
            window.location.protocol + "//" + 
            window.location.host + 
            window.location.pathname + 
            "?page=" + page + 
            "&searchParameter=" + searchParameter + 
            "&sortingOption=" + sortingOption + 
            "#sortingOption";
        
        window.location.href = uri;
    });
}

AddEventListenerToChangeProductSorting();