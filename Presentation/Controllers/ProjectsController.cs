using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
public class ProjectsController(IPortfolioRepository portfolioRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var projects = await portfolioRepository.GetProjectsAsync();

        return View(projects);
    }

    public async Task<IActionResult> Details(int id)
    {
        var project = await portfolioRepository.GetProjectByIdAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        return View(project);
    }
}