using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class TimelineController(IPortfolioRepository portfolioRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var timelineItems = await portfolioRepository.GetTimelineItemsAsync();

        return View(timelineItems);
    }
}