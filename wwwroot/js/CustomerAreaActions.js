$(document).ready(function()
{
    ChangeProductSorting();
});

function ChangeProductSorting()
{
    $("#sortingOption").change(function()
    {
        var page = 1;
        var searchParameter = "";
        var sortingOption = $(this).val();

        var queryString = new URLSearchParams(window.location.search);
        
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