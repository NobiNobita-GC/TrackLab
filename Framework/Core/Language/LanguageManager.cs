using System.Globalization;
using System.Resources;

namespace Core.Language
{
    public static class LanguageManager
    {
        private static string _languageType = "en";

        private static readonly ResourceManager phraseResourceManager =
            new("Core.Language.Lang-Phrase", typeof(LanguageManager).Assembly);

        private static readonly ResourceManager resourceManager =
            new("Core.Language.Lang-Word", typeof(LanguageManager).Assembly);

        public static event Action? LanguageChanged;

        public static string GetString(string key)
        {
            key = key.Replace(" ", string.Empty);

            CultureInfo culture = new(_languageType);

            string? phrase = phraseResourceManager.GetString(key, culture);
            if (!string.IsNullOrEmpty(phrase))
            {
                return phrase;
            }

            string? value = resourceManager.GetString(key, new CultureInfo(_languageType));
            return string.IsNullOrEmpty(value) ? key : value;
        }

        public static void ChangeLanguage(string cultureCode)
        {
            if (cultureCode.Equals("cn", StringComparison.OrdinalIgnoreCase)
                || cultureCode.Equals("zh-CN", StringComparison.OrdinalIgnoreCase))
            {
                _languageType = "zh-CN";
            }
            else
            {
                _languageType = "en";
            }
            LanguageChanged?.Invoke();
        }

        public static string GetLanguage()
        {
            return _languageType.ToUpper();
        }
    }
}
