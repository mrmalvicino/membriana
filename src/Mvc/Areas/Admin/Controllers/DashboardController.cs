using Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Filters;
using Mvc.Services.Api.Interfaces;
using System.Globalization;

namespace Mvc.Areas.Admin.Controllers;

/// <summary>
/// Controlador principal para el Dashboard del Back Office (Admin).
/// </summary>
[Area("Admin")]
[JwtAuthorizationFilter]
public class DashboardController : Controller
{
    private readonly IMemberStatusApiService _memberStatusApi;
    private readonly IUserApiService _userApi;

    public DashboardController(
        IUserApiService userService,
        IMemberStatusApiService memberStatusApi
    )
    {
        _userApi = userService;
        _memberStatusApi = memberStatusApi;
    }

    /// <summary>
    /// Muestra el Dashboard.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var loggedUserContext = await _userApi.GetLoggedUserContextAsync();
        var months = GetLastSixMonths(DateTime.Today);

        var activeMembersByMonth = await GetMemberCountsByMonthAsync(
            loggedUserContext.OrganizationId,
            months,
            MemberStatus.Active
        );
        
        var inactiveMembersByMonth = await GetMemberCountsByMonthAsync(
            loggedUserContext.OrganizationId,
            months,
            MemberStatus.Inactive
        );

        var dashboard = new DashboardViewModel
        {
            OrganizationName = loggedUserContext.OrganizationName,
            ActiveMembersCount = activeMembersByMonth[^1],
            ActiveMembersVariationPercent = CalculateVariationPercent(activeMembersByMonth),
            InactiveMembersCount = inactiveMembersByMonth[^1],
            InactiveMembersVariationPercent = CalculateVariationPercent(inactiveMembersByMonth),
            MonthlySignupsCount = 18,
            MonthlySignupsVariationPercent = 20m,
            MonthlyCancellationsCount = 7,
            MonthlyCancellationsVariationPercent = 75m,
            MonthlyIncome = 1_250_000m,
            MonthlyIncomeVariationPercent = -12.3m,
            Months = months.Select(GetMonthLabel).ToList(),
            ActiveMembersByMonth = activeMembersByMonth,
            SignupsByMonth = new() { 12, 18, 22, 15, 19, 18 },
            CancellationsByMonth = new() { 5, 7, 6, 9, 8, 7 }
        };

        return View(dashboard);
    }

    private async Task<List<int>> GetMemberCountsByMonthAsync(
        int organizationId,
        List<DateTime> months,
        MemberStatus status
    )
    {
        var counts = new List<int>(months.Count);

        foreach (var month in months)
        {
            counts.Add(
                await _memberStatusApi.CountMembersWithStatusAsync(
                    organizationId,
                    month.Year,
                    month.Month,
                    status
                )
            );
        }

        return counts;
    }

    private static List<DateTime> GetLastSixMonths(DateTime referenceDate)
    {
        var firstDayOfCurrentMonth = new DateTime(referenceDate.Year, referenceDate.Month, 1);

        return Enumerable.Range(0, 6)
            .Select(offset => firstDayOfCurrentMonth.AddMonths(offset - 5))
            .ToList();
    }

    private static string GetMonthLabel(DateTime month)
    {
        var culture = CultureInfo.GetCultureInfo("es-AR");
        var abbreviatedMonth = culture.DateTimeFormat.GetAbbreviatedMonthName(month.Month);
        abbreviatedMonth = abbreviatedMonth.TrimEnd('.');

        return culture.TextInfo.ToTitleCase(abbreviatedMonth);
    }

    private static decimal CalculateVariationPercent(IReadOnlyList<int> counts)
    {
        if (counts.Count < 2)
        {
            return 0;
        }

        var current = counts[^1];
        var previous = counts[^2];

        if (previous == 0)
        {
            return current == 0 ? 0 : 100;
        }

        return Math.Round(((decimal)(current - previous) / previous) * 100, 1);
    }
}
