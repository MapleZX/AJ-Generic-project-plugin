using UnityEngine;
using Firebase.Analytics;

namespace AJ.Generic.Service
{
    public class AJFirebase
    {
        public static void Log(string name)
        {
            Debug.Log(name);
        }
        public static void LogFormat(string format, params object[] args)
        {
            var log = System.String.Format(format, args);
            Debug.LogFormat(format, args);
        }
        public static void LogEvent(string name)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
        }
        public static void LogEvent(string name, params Parameter[] parameters)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameters);
        }
        public static void LogEvent(string name, string parameterName, int parameterValue)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public static void LogEvent(string name, string parameterName, long parameterValue)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public static void LogEvent(string name, string parameterName, double parameterValue)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
        public static void LogEvent(string name, string parameterName, string parameterValue)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
    }
}
