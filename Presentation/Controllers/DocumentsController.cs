using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class DocumentsController(IPortfolioRepository portfolioRepository) : Controller
{
    public async Task<IActionResult> DownloadCv()
    {
        var currentCv = await portfolioRepository.GetCurrentCvAsync();

        if (currentCv is null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            currentCv.FilePath);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

        return File(
            fileBytes,
            "application/pdf",
            currentCv.FileName);
    }
}