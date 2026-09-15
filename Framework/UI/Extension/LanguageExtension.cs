using Core.Language;
using System.Windows;
using System.Windows.Controls;

namespace UI.Extension
{
    public static class LanguageExtension
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached(
                "Text",
                typeof(string),
                typeof(LanguageExtension),
                new PropertyMetadata(string.Empty, OnTextChanged));

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string key = GetText(d);

            if (string.IsNullOrEmpty(key))
                return;

            string translatedText = LanguageManager.GetString(key);

            if (d is TextBlock textBlock)
            {
                textBlock.Text = translatedText;
            }
        }
    }
}
