using ClientPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientPortal.Pages.Account;

public class SubscriptionModel(UserManager<ApplicationUser> userManager) : PageModel
{
    public SubscriptionPlan CurrentPlan { get; set; }
    public DateTime? SubscribedOn { get; set; }
    [TempData] public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();
        CurrentPlan = user.Plan;
        SubscribedOn = user.SubscribedOn;
        return Page();
    }

    public async Task<IActionResult> OnPostChooseAsync(SubscriptionPlan plan)
    {
        if (plan == SubscriptionPlan.None || !Enum.IsDefined(plan)) return BadRequest();
        return await UpdateAsync(plan, $"Je abonnement is gewijzigd naar {plan}.");
    }

    public Task<IActionResult> OnPostCancelAsync() =>
        UpdateAsync(SubscriptionPlan.None, "Je abonnement is opgezegd.");

    private async Task<IActionResult> UpdateAsync(SubscriptionPlan plan, string message)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();
        user.Plan = plan;
        user.SubscribedOn = plan == SubscriptionPlan.None ? null : DateTime.UtcNow;
        await userManager.UpdateAsync(user);
        StatusMessage = message;
        return RedirectToPage();
    }
}
