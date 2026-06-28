using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

[Route("Admin/Profile")]
[Authorize]
public class AdminProfileController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var profile = await portfolioRepository.GetProfileAsync();

        return View("~/Views/Admin/Profile/Index.cshtml", profile);
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit()
    {
        var profile = await portfolioRepository.GetProfileAsync();

        if (profile is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Profile/Edit.cshtml", profile);
    }

    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PortfolioProfile profile)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Profile/Edit.cshtml", profile);
        }

        await portfolioRepository.UpdateProfileAsync(profile);

        return RedirectToAction(nameof(Index));
    }
}