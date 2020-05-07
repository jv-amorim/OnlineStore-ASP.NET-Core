$(document).ready(function() 
{
    $(".btn-delete").click(function(e) 
    {
        var result = confirm("Do you really want to delete this item?");

        if (!result)
        {
            e.preventDefault();
        }
    });
});