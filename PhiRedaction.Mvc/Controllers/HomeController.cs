using Microsoft.AspNetCore.Mvc;
using PhiRedaction.Core.Models.Redaction;
using PhiRedaction.Core.Services.Redaction;
using PhiRedaction.Mvc.Models;
using System.Diagnostics;
using System.IO.Compression;

namespace PhiRedaction.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRedactionService _redactionService;

        public HomeController(ILogger<HomeController> logger,
            IRedactionService redactionService)
        {
            _logger = logger;
            _redactionService = redactionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LabOrderFiles model)
        {
            _logger.LogInformation("Processing files...");
            if (ModelState.IsValid)
            {
                try
                {
                    var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var inputFilePaths = new List<string>();
                    foreach (var file in model.Files)
                    {
                        if (file != null)
                        {
                            var filePath = Path.Combine(dir, file.File.FileName);
                            inputFilePaths.Add(filePath);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.File.CopyTo(fileStream);
                            }
                        }
                    }

                    var redactionResults = new List<RedactionResult>();
                    foreach (var inputFile in inputFilePaths)
                    {
                        var result = await _redactionService.RedactAsync(inputFile);
                        result.InputFilePath = inputFile;
                        redactionResults.Add(result);
                    }
                    model.Results = redactionResults;

                    if (redactionResults.Count > 1)
                    {
                        var zipFilePath = Path.Combine(dir, "redacted_files.zip");

                        if (System.IO.File.Exists(zipFilePath))
                        {
                            System.IO.File.Delete(zipFilePath);
                        }
                        using (var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                        {
                            foreach (var result in redactionResults)
                            {
                                archive.CreateEntryFromFile(result.OutputFilePath, Path.GetFileName(result.OutputFilePath));
                            }
                        }

                        var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var relativePath = Path.GetRelativePath(webRootPath, zipFilePath).Replace("\\", "/");
                        var virtualPath = "/" + relativePath;
                        return File(virtualPath, "application/zip", "redacted_files.zip");
                    }
                    else
                    {
                        var result = redactionResults.FirstOrDefault();

                        var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var relativePath = Path.GetRelativePath(webRootPath, result.OutputFilePath).Replace("\\", "/");
                        var virtualPath = "/" + relativePath;
                        return File(virtualPath, "application/octet-stream", Path.GetFileName(result.OutputFilePath));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Sorry, an error occurred while processing the files. Please try again");
                    ModelState.AddModelError(string.Empty, ex.Message);
                    _logger.LogError($"Error: {ex.Message}");
                }

            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
