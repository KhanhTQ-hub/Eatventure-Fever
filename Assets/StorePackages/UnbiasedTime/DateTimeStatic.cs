using System;
using com.unimob.services.datetime;

public static class DateTimeStatic
{
    private static IDateTimeService DateTimeService => Game.Instance?.GetService<IDateTimeService>();

    public static DateTime UtcNow
    {
        get
        {
            DateTime nowUtc = DateTimeService?.NowUtc ?? DateTime.UtcNow;
#if UNITY_EDITOR || DEVELOPMENT || STAGING
            nowUtc = nowUtc.AddDays(_days).AddHours(_hours).AddMinutes(_minutes).AddSeconds(_seconds);
#endif
            return nowUtc;
        }
    }

    public static DateTime Now => DateTimeService?.Now ?? DateTime.Now;

    private static int _days;
    private static int _hours;
    private static int _minutes;
    private static int _seconds;

    public static void NowAddDays(int d = 1)
    {
        _days += d;
    }

    public static void NowAddHours(int h = 1)
    {
        _hours += h;
    }

    public static void NowAddMinutes(int m = 1)
    {
        _minutes += m;
    }

    public static void NowAddSeconds(int s = 1)
    {
        _seconds += s;
    }

    public static void SetReset(bool reset)
    {
        if (reset)
        {
            _days = 0;
            _hours = 0;
            _minutes = 0;
            _seconds = 0;
        }
    }
}