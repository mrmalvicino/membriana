using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api.Helpers;

internal static class DbUpdateExceptionHelper
{
    public static bool TryCreateConflictMessage(DbUpdateException exception, out string message)
    {
        if (!IsUniqueConstraintViolation(exception, out var sqlMessage))
        {
            message = string.Empty;
            return false;
        }

        if (sqlMessage.Contains("IX_MembershipPlans_OrganizationId_Name", StringComparison.OrdinalIgnoreCase))
        {
            message = "Ya existe un plan de membresía con ese nombre.";
            return true;
        }

        if (sqlMessage.Contains("IX_Employees_OrganizationId_UserId", StringComparison.OrdinalIgnoreCase))
        {
            message = "Ya existe un empleado asociado a ese usuario en la organización.";
            return true;
        }

        message = "Ya existe un recurso con esos datos.";
        return true;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception, out string sqlMessage)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            sqlMessage = sqlException.Message;
            return sqlException.Number is 2601 or 2627;
        }

        sqlMessage = string.Empty;
        return false;
    }
}
