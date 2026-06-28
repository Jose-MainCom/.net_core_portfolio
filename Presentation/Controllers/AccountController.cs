using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Account;

namespace Presentation.Controllers;

public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var signInResult = await signInManager.PasswordSignInAsync(
            loginViewModel.Email,
            loginViewModel.Password,
            loginViewModel.RememberMe,
            lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginViewModel);
        }

        if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl) &&
            Url.IsLocalUrl(loginViewModel.ReturnUrl))
        {
            return Redirect(loginViewModel.ReturnUrl);
        }

        return RedirectToAction("Index", "Admin");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var viewModel = new AccountSettingsViewModel
        {
            Email = user.Email ?? string.Empty
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(AccountSettingsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        if (!string.Equals(user.Email, viewModel.Email, StringComparison.OrdinalIgnoreCase))
        {
            user.Email = viewModel.Email;
            user.UserName = viewModel.Email;
            user.EmailConfirmed = true;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(viewModel);
            }
        }

        if (!string.IsNullOrWhiteSpace(viewModel.NewPassword))
        {
            var passwordResult = await userManager.ChangePasswordAsync(
                user,
                viewModel.CurrentPassword,
                viewModel.NewPassword);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(viewModel);
            }
        }

        await signInManager.RefreshSignInAsync(user);

        return RedirectToAction("Index", "Admin");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await userManager.FindByEmailAsync(viewModel.Email);

        if (user is null)
        {
            TempData["SuccessMessage"] = "If the email exists, a password reset link has been prepared.";
            return RedirectToAction(nameof(Login));
        }

        if (user.LastPasswordResetEmailSentAtUtc.HasValue &&
            user.LastPasswordResetEmailSentAtUtc.Value.Date == DateTime.UtcNow.Date)
        {
            TempData["SuccessMessage"] = "A password reset link has already been requested today.";
            return RedirectToAction(nameof(Login));
        }

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = Url.Action(
            nameof(ResetPassword),
            "Account",
            new
            {
                userId = user.Id,
                token = resetToken
            },
            Request.Scheme);

        user.LastPasswordResetEmailSentAtUtc = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine();
        Console.WriteLine("==============================================");
        Console.WriteLine(" PASSWORD RESET REQUEST");
        Console.WriteLine("==============================================");
        Console.WriteLine(resetUrl);
        Console.WriteLine("==============================================");
        Console.WriteLine();

        Console.ResetColor();

        TempData["SuccessMessage"] = "If the email exists, a password reset link has been prepared.";

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(token))
        {
            return RedirectToAction(nameof(Login));
        }

        return View(new ResetPasswordViewModel
        {
            UserId = userId,
            Token = token
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await userManager.FindByIdAsync(viewModel.UserId);

        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var resetResult = await userManager.ResetPasswordAsync(
            user,
            viewModel.Token,
            viewModel.Password);

        if (!resetResult.Succeeded)
        {
            foreach (var error in resetResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);
        }

        TempData["SuccessMessage"] = "Password has been reset successfully.";

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}