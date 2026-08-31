using Domain.Shared;

namespace Domain.Errors;

public static class ReportErrors
{
    public static class Codes
    {
        public const string InvalidEndDate = "Report.InvalidEndDate";
    }

    public static class Helpers
    {
        public static readonly Error InvalidEndDate = new(Codes.InvalidEndDate, $"End date must be greater than start date.");
    }
}