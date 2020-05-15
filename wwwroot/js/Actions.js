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
    $('.money').mask('000,000,000,000,000.00', {reverse: true});
});