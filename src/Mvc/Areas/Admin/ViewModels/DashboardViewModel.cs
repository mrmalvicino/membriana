namespace Mvc.Areas.Admin.ViewModels;

public class DashboardViewModel
{
    public int ActiveMembersCount { get; set; }
    public decimal ActiveMembersVariationPercent { get; set; }

    public int InactiveMembersCount { get; set; }
    public decimal InactiveMembersVariationPercent { get; set; }

    public int MonthlySignupsCount { get; set; }
    public decimal MonthlySignupsVariationPercent { get; set; }

    public int MonthlyCancellationsCount { get; set; }
    public decimal MonthlyCancellationsVariationPercent { get; set; }

    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyIncomeVariationPercent { get; set; }

    public List<string> Months { get; set; } = new();
    public List<int> ActiveMembersByMonth { get; set; } = new();
    public List<int> SignupsByMonth { get; set; } = new();
    public List<int> CancellationsByMonth { get; set; } = new();
}
