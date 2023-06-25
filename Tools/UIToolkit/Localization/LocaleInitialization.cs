using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using AJ.Generic.Tools.Keys;

namespace AJ.Generic.Tools
{
    public class LocaleInitialization
    {
        public const string LocaleSettingsKey = "locale key";
        public static IEnumerator ISystemLocale(Action<CountryCode> callback = null)
        {
            yield return LocalizationSettings.InitializationOperation;
            if (PlayerPrefs.HasKey(LocaleSettingsKey))
            {
                var code = PlayerPrefs.GetInt(LocaleSettingsKey);
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[code];
                callback?.Invoke((CountryCode)code);
            } else
            {
                var locale = LocalizationSettings.SelectedLocale;
                var code = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
                PlayerPrefs.SetInt(LocaleSettingsKey, code);
                callback?.Invoke((CountryCode)code);
            }
        }
        public static CountryCode SystemLocale()
        {
            var locale = LocalizationSettings.SelectedLocale;
            var code = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
            if (PlayerPrefs.HasKey(LocaleSettingsKey))
            {
                code = PlayerPrefs.GetInt(LocaleSettingsKey);
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[code];
            }
            else
            {              
                PlayerPrefs.SetInt(LocaleSettingsKey, code);
            }
            return (CountryCode)code;
        }
    }
}
