namespace Tgs.GameInterface.Cards365.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset StringConvertToDatetime(this string str)
    {
        var _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "China Standard Time", baseUtcOffset: TimeSpan.FromHours(+8), "China Standard Time", "China Standard Time");
        if (string.IsNullOrEmpty(str))
        {
            return DateTime.UnixEpoch;
        }
        var date = DateTime.Parse(str);

        return new DateTimeOffset(date, _timeZone.GetUtcOffset(date));
    }
}
