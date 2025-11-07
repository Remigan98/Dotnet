using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers;

[ApiController, Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    [HttpGet, ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
    public ActionResult GetFile([FromQuery] string fileName)
    {
        string rootPath = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(rootPath, "PrivateFiles", fileName);

        bool fileExists = System.IO.File.Exists(filePath);

        if (fileExists == false)
        {
            return NotFound("File not found.");
        }

        FileExtensionContentTypeProvider contentProvider = new FileExtensionContentTypeProvider();

        if (contentProvider.TryGetContentType(fileName, out string? contentType) == false)
        {
            contentType = "application/octet-stream";
        }

        byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

        return File(fileContents, contentType, fileName);
    }

    [HttpPost, ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult Upload([FromForm]IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string fileName = file.FileName;
            string filePath = Path.Combine(rootPath, "PrivateFiles", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok();
        }

        return BadRequest("No file uploaded.");
    }
}

