using Contracts.Enums;

namespace Mvc.Areas.Portal.ViewModels;

public class DashboardViewModel
{
    public string MemberFullName { get; set; } = "";
    public string? Document { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string OrganizationName { get; set; } = "";
    public string MembershipPlanName { get; set; } = "";
    public DateTime AdmissionDate { get; set; }

    public MemberStatus Status { get; set; }
    public PaymentSummaryViewModel? LastPayment { get; set; }
}

public class PaymentSummaryViewModel
{
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}
