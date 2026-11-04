using Microsoft.AspNetCore.Identity;

namespace ClientPortal.Data;

public enum SubscriptionPlan
{
    None = 0,
    Basic = 1,
    Standard = 2,
    Premium = 3
}

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.None;
    public DateTime? SubscribedOn { get; set; }
}
