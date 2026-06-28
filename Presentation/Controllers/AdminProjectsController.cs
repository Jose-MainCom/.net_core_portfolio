using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("Admin/Projects")]
[Authorize]
public class AdminProjectsController(IPortfolioRepository portfolioRepository) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var projects = await portfolioRepository.GetProjectsAsync();

        return View("~/Views/Admin/Projects/Index.cshtml", projects);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Projects/Create.cshtml");
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PortfolioProject project)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/Create.cshtml", project);
        }

        project.CreatedDate = DateTime.UtcNow;

        await portfolioRepository.AddProjectAsync(project);

        TempData["SuccessMessage"] = "Project created successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await portfolioRepository.GetProjectByIdAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Projects/Edit.cshtml", project);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PortfolioProject project)
    {
        if (id != project.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/Edit.cshtml", project);
        }

        await portfolioRepository.UpdateProjectAsync(project);

        TempData["SuccessMessage"] = "Project updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await portfolioRepository.GetProjectByIdAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Projects/Delete.cshtml", project);
    }

    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await portfolioRepository.DeleteProjectAsync(id);

        TempData["SuccessMessage"] = "Project deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{projectId:int}/Links/Create")]
    public async Task<IActionResult> CreateLink(int projectId)
    {
        var project = await portfolioRepository.GetProjectByIdAsync(projectId);

        if (project is null)
        {
            return NotFound();
        }

        var projectLink = new ProjectLink
        {
            PortfolioProjectId = projectId
        };

        return View("~/Views/Admin/Projects/CreateLink.cshtml", projectLink);
    }

    [HttpPost("{projectId:int}/Links/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateLink(int projectId, ProjectLink projectLink)
    {
        if (projectId != projectLink.PortfolioProjectId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/CreateLink.cshtml", projectLink);
        }

        await portfolioRepository.AddProjectLinkAsync(projectLink);

        TempData["SuccessMessage"] = "Project link created successfully.";

        return RedirectToAction(nameof(Edit), new { id = projectId });
    }

    [HttpGet("Links/Edit/{id:int}")]
    public async Task<IActionResult> EditLink(int id)
    {
        var projectLink = await portfolioRepository.GetProjectLinkByIdAsync(id);

        if (projectLink is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Projects/EditLink.cshtml", projectLink);
    }

    [HttpPost("Links/Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLink(int id, ProjectLink projectLink)
    {
        if (id != projectLink.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/EditLink.cshtml", projectLink);
        }

        await portfolioRepository.UpdateProjectLinkAsync(projectLink);

        TempData["SuccessMessage"] = "Project link updated successfully.";

        return RedirectToAction(nameof(Edit), new
        {
            id = projectLink.PortfolioProjectId
        });
    }

    [HttpGet("{projectId:int}/Images/Create")]
    public async Task<IActionResult> CreateImage(int projectId)
    {
        var project = await portfolioRepository.GetProjectByIdAsync(projectId);

        if (project is null)
        {
            return NotFound();
        }

        var projectImage = new ProjectImage
        {
            PortfolioProjectId = projectId
        };

        return View("~/Views/Admin/Projects/CreateImage.cshtml", projectImage);
    }

    [HttpPost("{projectId:int}/Images/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateImage(
        int projectId,
        ProjectImage projectImage,
        IFormFile imageFile)
    {
        if (projectId != projectImage.PortfolioProjectId)
        {
            return BadRequest();
        }

        if (imageFile.Length == 0)
        {
            ModelState.AddModelError(nameof(imageFile), "Please select a valid image file.");
        }

        ModelState.Remove(nameof(projectImage.FileName));
        ModelState.Remove(nameof(projectImage.FilePath));

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/CreateImage.cshtml", projectImage);
        }

        var imagesFolderPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "images",
            "projects");

        Directory.CreateDirectory(imagesFolderPath);

        var fileExtension = Path.GetExtension(imageFile.FileName);
        var fileName = $"project-{projectId}-{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
        var fullFilePath = Path.Combine(imagesFolderPath, fileName);

        await using var fileStream = new FileStream(fullFilePath, FileMode.Create);
        await imageFile.CopyToAsync(fileStream);

        projectImage.FileName = fileName;
        projectImage.FilePath = $"images/projects/{fileName}";
        projectImage.UploadedDate = DateTime.UtcNow;

        await portfolioRepository.AddProjectImageAsync(projectImage);

        TempData["SuccessMessage"] = "Project image uploaded successfully.";

        return RedirectToAction(nameof(Edit), new { id = projectId });
    }

    [HttpGet("Images/Edit/{id:int}")]
    public async Task<IActionResult> EditImage(int id)
    {
        var projectImage = await portfolioRepository.GetProjectImageByIdAsync(id);

        if (projectImage is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Projects/EditImage.cshtml", projectImage);
    }

    [HttpPost("Images/Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditImage(int id, ProjectImage projectImage)
    {
        if (id != projectImage.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/Projects/EditImage.cshtml", projectImage);
        }

        var existingProjectImage = await portfolioRepository.GetProjectImageByIdAsync(id);

        if (existingProjectImage is null)
        {
            return NotFound();
        }

        existingProjectImage.AltText = projectImage.AltText;
        existingProjectImage.DisplayOrder = projectImage.DisplayOrder;
        existingProjectImage.IsCoverImage = projectImage.IsCoverImage;

        await portfolioRepository.UpdateProjectImageAsync(existingProjectImage);

        TempData["SuccessMessage"] = "Project image updated successfully.";

        return RedirectToAction(nameof(Edit), new
        {
            id = existingProjectImage.PortfolioProjectId
        });
    }

    [HttpGet("Images/Delete/{id:int}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var projectImage = await portfolioRepository.GetProjectImageByIdAsync(id);

        if (projectImage is null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Projects/DeleteImage.cshtml", projectImage);
    }

    [HttpPost("Images/Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImageConfirmed(int id)
    {
        var projectImage = await portfolioRepository.GetProjectImageByIdAsync(id);

        if (projectImage is null)
        {
            return NotFound();
        }

        var projectId = projectImage.PortfolioProjectId;

        await portfolioRepository.DeleteProjectImageAsync(id);

        TempData["SuccessMessage"] = "Project image deleted successfully.";

        return RedirectToAction(nameof(Edit), new
        {
            id = projectId
        });
    }
}