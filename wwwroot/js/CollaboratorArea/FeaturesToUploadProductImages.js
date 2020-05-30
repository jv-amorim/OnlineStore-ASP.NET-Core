function AddFeaturesToUploadProductImages()
{
    $(".img-upload").click(function()
    {
        $(this).parent().find(".img-file-input").click();
    });

    $(".img-file-input").change(function() 
    {
        var form = new FormData();
        var binaryImageData = $(this)[0].files[0];
        form.append("file", binaryImageData);

        var imageField = $(this).parent().find(".img-upload");
        var imageFilePathField = $(this).parent().find("input[name=imageFilePath]");
        var deleteButton = $(this).parent().find(".btn-delete-image");
        
        imageField.attr("src", "/img/loading-animation.gif");

        $.ajax({
            type: "POST",
            url: "/Collaborator/Image/Save",
            data: form,
            contentType: false,
            processData: false,
            error: function() 
            {
                alert("Error uploading image. Try again.");
                imageField.attr("src", "/img/default-image.png");
            },
            success: function(data) 
            {
                var filePath = data.filePath;
                imageField.attr("src", filePath);
                imageFilePathField.val(filePath);
                deleteButton.css("display", "inline-block");
            }
        });
    });

    $(".btn-delete-image").click(function()
    {
        var imageField = $(this).parent().find(".img-upload");
        var imageFilePathField = $(this).parent().find("input[name=imageFilePath]");
        var fileInput = $(this).parent().find(".img-file-input");
        var deleteButton = $(this).parent().find(".btn-delete-image");

        $.ajax({
            type: "GET",
            url: "/Collaborator/Image/Delete?filePath=" + imageFilePathField.val(),
            error: function() 
            {
                alert("Error uploading image. Try again.");
            },
            success: function() 
            {
                imageField.attr("src", "/img/default-image.png");
                imageFilePathField.val("");
                fileInput.val("");
                deleteButton.css("display", "none");
            }
        });
    });
}

AddFeaturesToUploadProductImages();