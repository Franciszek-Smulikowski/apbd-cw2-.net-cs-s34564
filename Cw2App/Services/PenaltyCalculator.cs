using System;

namespace Cw2App.Services;

public class PenaltyCalculator
{
    private const decimal DailyPenaltyRate = 5.0m;

    public decimal CalculatePenalty(DateTime dueDate, DateTime returnedAt)
    {
        if (returnedAt <= dueDate)
        {
            return 0m;
        }

        var delay = returnedAt.Date - dueDate.Date;
        var delayedDays = (int)Math.Ceiling(delay.TotalDays); // Każdy rozpoczęty dzień opóźnienia

        return delayedDays > 0 ? delayedDays * DailyPenaltyRate : 0m;
    }
}
