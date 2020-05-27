$(document).ready(function()
{
    ChangeProductSorting();
    ChangeProductFeaturedImage();
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

function ChangeProductFeaturedImage()
{
    $(".img-small-wrap img").click(function(){
        var imagePath = $(this).attr("src");
        $(".img-big-wrap a").attr("href", imagePath);
        $(".img-big-wrap img").attr("src", imagePath);
    });
}