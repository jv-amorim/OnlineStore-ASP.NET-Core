$(document).ready(function() 
{
    $(".btn-danger").click(function(e) 
    {
        var result = confirm("Do you really want to do this action?");

        if (!result)
        {
            e.preventDefault();
        }
    });
});