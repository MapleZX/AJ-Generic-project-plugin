using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Extension
{
    public enum DateStatus { Earlier, Same, Later }
    public static class AJDateTimeExtension
    {
        public static string TimerFormat(this double seconds)
        {
            var clockStr = "00:00";
            if (seconds <= 0) return clockStr;
            var clock = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            var Hours = clock.Hours.CountdownFormat(2);
            var Minutes = clock.Minutes.CountdownFormat(2);
            var Seconds = clock.Seconds.CountdownFormat(2);
            clockStr = String.Format("{0}:{1}:{2}", Hours, Minutes, Seconds);
            return clockStr;
        }
        public static string CountdownFormat(this int cd, int count)
        {
            var tab = "";
            var length = cd.ToString().Length;
            for (int i = 0; i < count - length; i++)
            {
                tab += "0";
            }
            var clock = cd < Mathf.Pow(10, length) ? tab + cd : cd.ToString();
            return clock;
        }
        public static string GetDateString(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        public static DateTime GetDate(this string date)
            => DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        public static double GetTimeDifferenceSeconds(this string current, string last)
            => GetTimeDifferenceSeconds(current.GetDate(), last.GetDate());
        public static double GetTimeDifferenceSeconds(this DateTime current, DateTime last)
        {
            var diff = current.Subtract(last);
            return diff.TotalSeconds;
        }
        public static double GetTimeDifferenceMinutes(this string current, string last)
            => GetTimeDifferenceMinutes(current.GetDate(), last.GetDate());
        public static double GetTimeDifferenceMinutes(this DateTime current, DateTime last)
        {
            var diff = current.Subtract(last);
            return diff.TotalMinutes;
        }
        public static double GetTimeDifferenceHours(this string current, string last)
            => GetTimeDifferenceDays(current.GetDate(), last.GetDate());
        public static double GetTimeDifferenceHours(this DateTime current, DateTime last)
        {
            var diff = current.Subtract(last);
            return diff.TotalHours;
        }
        public static double GetTimeDifferenceDays(this string current, string last)
            => GetTimeDifferenceDays(current.GetDate(), last.GetDate());
        public static double GetTimeDifferenceDays(this DateTime current, DateTime last)
        {
            var diff = current.Subtract(last);
            return diff.TotalDays;
        }
        public static DateStatus DateTimeCompare(this DateTime current, DateTime other)
        {
            var result = DateTime.Compare(current, other);
            if (result < 0)
            {
                // var relationship = "is earlier than";
                return DateStatus.Earlier;
            }
            else if (result == 0)
            {
                // var relationship = "is the same time as";
                return DateStatus.Same;
            }
            else
            {
                // var relationship = "is later than";
                return DateStatus.Later;
            }
        }
        public static string ToNumberFormat(this int number)
        {
            return ToNumberFormat((double)number);
        }
        public static string ToNumberFormat(this float number)
        {
            return ToNumberFormat((double)number);
        }
        public static string ToNumberFormat(this double number)
        {
            var numerator = Mathf.Pow(10, 0);
            var signs = "";
            if (number.ToString().Length < 4)
            {
                return number.ToString();
            }
            else if (number.ToString().Length >= 4 && number.ToString().Length < 7)
            {
                numerator = Mathf.Pow(10, 3);
                signs = "K";
            }
            else if (number.ToString().Length >= 7 && number.ToString().Length < 10)
            {
                numerator = Mathf.Pow(10, 6);
                signs = "M";
            }
            else if (number.ToString().Length >= 10)
            {
                numerator = Mathf.Pow(10, 9);
                signs = "B";
            }
            return (number / numerator).ToString("0.0") + signs;
        }
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);
    }
}