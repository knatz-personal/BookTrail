namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Converts a UTC DateTime to local time based on the specified time zone identifier.
        /// </summary>
        /// <param name="utcDateTime">The UTC DateTime to be converted.</param>
        /// <param name="timeZoneId">The identifier of the target time zone.</param>
        /// <returns>The DateTime converted to the specified local time zone.</returns>
        public static DateTime ToLocal(this DateTime utcDateTime, string timeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }

        /// <summary>
        ///     Converts a local DateTime to UTC based on the specified time zone.
        /// </summary>
        /// <param name="localDateTime">The local DateTime to convert.</param>
        /// <param name="timeZoneId">The identifier of the time zone to use for conversion.</param>
        /// <returns>The UTC equivalent of the provided local DateTime.</returns>
        public static DateTime ToUtc(this DateTime localDateTime, string timeZoneId)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);
        }

        /// <summary>
        ///     Converts a local DateTime to UTC.
        /// </summary>
        /// <param name="localDateTime">The local DateTime to convert.</param>
        /// <returns>The UTC equivalent of the provided local DateTime.</returns>
        public static DateTime Utc(this DateTime dt) => DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        /// <summary>
        ///     Returns a timestamp string
        /// </summary>
        public static string ToTimestamp(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmssffffff");
        }

        /// <summary>
        ///     Set date portion of a datetime.
        ///     Returns the modified datetime.
        /// </summary>
        public static DateTime SetDatePortion(this DateTime currentDatetime, DateTime newDate)
        {
            return newDate.Date + currentDatetime.TimeOfDay;
        }
    }
}