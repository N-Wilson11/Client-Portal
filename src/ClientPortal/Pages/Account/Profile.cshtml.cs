using System.ComponentModel.DataAnnotations;
using ClientPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientPortal.Pages.Account;

public class ProfileModel(UserManager<ApplicationUser> userManager) : PageModel
{
    public string? Email { get; set; }
    [TempData] public string? StatusMessage { get; set; }
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [StringLength(100)] public string? FullName { get; set; }
        [Phone] public string? PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();
        Email = user.Email;
        Input = new InputModel { FullName = user.FullName, PhoneNumber = user.PhoneNumber };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();
        Email = user.Email;
        if (!ModelState.IsValid) return Page();
        user.FullName = Input.FullName;
        user.PhoneNumber = Input.PhoneNumber;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
            return Page();
        }
        StatusMessage = "Profiel bijgewerkt.";
        return RedirectToPage();
    }
}
