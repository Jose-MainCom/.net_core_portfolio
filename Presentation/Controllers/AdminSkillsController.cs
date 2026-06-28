using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers;

[Route("Admin/Skills")]
[Authorize]
public class AdminSkillsController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var skills = await portfolioRepository.GetSkillsAsync();
        return View("~/Views/Admin/Skills/Index.cshtml", skills);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Skills/Create.cshtml");
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Skill skill)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Skills/Create.cshtml", skill);
        }

        await portfolioRepository.AddSkillAsync(skill);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var skill = await portfolioRepository.GetSkillByIdAsync(id);

        if (skill is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Skills/Edit.cshtml", skill);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Skill skill)
    {
        if (id != skill.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Skills/Edit.cshtml", skill);
        }

        await portfolioRepository.UpdateSkillAsync(skill);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var skill = await portfolioRepository.GetSkillByIdAsync(id);

        if (skill is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Skills/Delete.cshtml", skill);
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await portfolioRepository.DeleteSkillAsync(id);
        return RedirectToAction(nameof(Index));
    }
}