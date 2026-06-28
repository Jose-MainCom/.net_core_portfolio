using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

[Route("Admin/Timeline")]
[Authorize]
public class AdminTimelineController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var timelineItems = await portfolioRepository.GetTimelineItemsAsync();
        return View("~/Views/Admin/Timeline/Index.cshtml", timelineItems);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Timeline/Create.cshtml");
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TimelineItem timelineItem)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Timeline/Create.cshtml", timelineItem);
        }

        await portfolioRepository.AddTimelineItemAsync(timelineItem);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var timelineItem = await portfolioRepository.GetTimelineItemByIdAsync(id);

        if (timelineItem is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Timeline/Edit.cshtml", timelineItem);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TimelineItem timelineItem)
    {
        if (id != timelineItem.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Timeline/Edit.cshtml", timelineItem);
        }

        await portfolioRepository.UpdateTimelineItemAsync(timelineItem);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var timelineItem = await portfolioRepository.GetTimelineItemByIdAsync(id);

        if (timelineItem is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Timeline/Delete.cshtml", timelineItem);
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await portfolioRepository.DeleteTimelineItemAsync(id);
        return RedirectToAction(nameof(Index));
    }
}