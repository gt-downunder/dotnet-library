namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="TimeSpan"/>.
    /// </summary>
    public static class TimeSpanEx
    {
        extension(TimeSpan timeSpan)
        {
            /// <summary>
            /// Converts the <see cref="TimeSpan"/> to a human-readable string such as "2 hours 5 minutes"
            /// or "3 days 1 hour".
            /// </summary>
            /// <returns>A human-readable duration string. Returns "0 seconds" for <see cref="TimeSpan.Zero"/>.</returns>
            public string ToHumanReadable()
            {
                if (timeSpan == TimeSpan.Zero)
                    return "0 seconds";

                TimeSpan abs = timeSpan < TimeSpan.Zero ? timeSpan.Negate() : timeSpan;
                var parts = new List<string>();

                if (abs.Days >= 365)
                {
                    int years = abs.Days / 365;
                    parts.Add($"{years} year{(years == 1 ? "" : "s")}");
                    abs -= TimeSpan.FromDays(years * 365);
                }

                if (abs.Days >= 30)
                {
                    int months = abs.Days / 30;
                    parts.Add($"{months} month{(months == 1 ? "" : "s")}");
                    abs -= TimeSpan.FromDays(months * 30);
                }

                if (abs.Days > 0)
                    parts.Add($"{abs.Days} day{(abs.Days == 1 ? "" : "s")}");

                if (abs.Hours > 0)
                    parts.Add($"{abs.Hours} hour{(abs.Hours == 1 ? "" : "s")}");

                if (abs.Minutes > 0)
                    parts.Add($"{abs.Minutes} minute{(abs.Minutes == 1 ? "" : "s")}");

                if (abs.Seconds > 0 && parts.Count == 0)
                    parts.Add($"{abs.Seconds} second{(abs.Seconds == 1 ? "" : "s")}");

                string result = string.Join(" ", parts);
                return timeSpan < TimeSpan.Zero ? $"-{result}" : result;
            }
        }
    }
}

