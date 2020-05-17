using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Libraries.Filters;
using OnlineStore.Libraries.FileUtils;

namespace OnlineStore.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAuthorization]
    public class ImageController : Controller
    {
        [HttpPost]
        public IActionResult Save(IFormFile file)
        {
            string filePath = FileManager.SaveProductImage(file);

            if (string.IsNullOrEmpty(filePath))
                return new StatusCodeResult(500);

            return Ok(new { filePath = filePath });
        }

        [HttpGet]
        public IActionResult Delete(string filePath)
        {
            bool wasTheFileDeleted = FileManager.DeleteProductImage(filePath);

            if (wasTheFileDeleted)
                return Ok();
            
            return BadRequest();
        }
    }
}