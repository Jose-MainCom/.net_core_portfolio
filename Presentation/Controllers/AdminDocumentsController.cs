using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

[Route("Admin/Documents")]
[Authorize]
public class AdminDocumentsController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var currentCv = await portfolioRepository.GetCurrentCvAsync();

        return View("~/Views/Admin/Documents/Index.cshtml", currentCv);
    }

    [HttpGet("UploadCv")]
    public IActionResult UploadCv()
    {
        return View("~/Views/Admin/Documents/UploadCv.cshtml");
    }

    [HttpPost("UploadCv")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadCv(IFormFile cvFile)
    {
        if (cvFile.Length == 0)
        {
            ModelState.AddModelError(nameof(cvFile), "Please select a valid PDF file.");

            return View("~/Views/Admin/Documents/UploadCv.cshtml");
        }

        var documentsFolderPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "documents");

        Directory.CreateDirectory(documentsFolderPath);

        var fileName = $"Fredric-Domvall-CV-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";

        var fullFilePath = Path.Combine(documentsFolderPath, fileName);

        await using var fileStream = new FileStream(fullFilePath, FileMode.Create);

        await cvFile.CopyToAsync(fileStream);

        var uploadedDocument = new UploadedDocument
        {
            FileName = fileName,
            FilePath = $"documents/{fileName}",
            UploadedDate = DateTime.UtcNow,
            DocumentType = DocumentType.CurriculumVitae
        };

        await portfolioRepository.AddUploadedDocumentAsync(uploadedDocument);

        return RedirectToAction(nameof(Index));
    }
}