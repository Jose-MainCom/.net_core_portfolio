using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class SkillsController(IPortfolioRepository portfolioRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var skills = await portfolioRepository.GetSkillsAsync();

        return View(skills);
    }
}