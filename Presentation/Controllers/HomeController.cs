using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
public class HomeController(IPortfolioRepository portfolioRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var profile = await portfolioRepository.GetProfileAsync();

        return View(profile);
    }
}