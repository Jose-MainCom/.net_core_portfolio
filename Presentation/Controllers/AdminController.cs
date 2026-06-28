using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Admin;

namespace Presentation.Controllers;

[Authorize]
[Route("Admin")]
public class AdminController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var currentCv = await portfolioRepository.GetCurrentCvAsync();

        var viewModel = new AdminDashboardViewModel
        {
            ProjectCount = await portfolioRepository.GetProjectCountAsync(),
            FeaturedProjectCount = await portfolioRepository.GetFeaturedProjectCountAsync(),
            SkillCount = await portfolioRepository.GetSkillCountAsync(),
            TimelineItemCount = await portfolioRepository.GetTimelineItemCountAsync(),
            ProjectImageCount = await portfolioRepository.GetProjectImageCountAsync(),
            HasCurrentCv = currentCv is not null
        };

        return View("~/Views/Admin/Index.cshtml", viewModel);
    }
}