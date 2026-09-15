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
            UpdateText(d);
            if (d is FrameworkElement element)
            {
                element.Loaded -= OnLoaded;
                element.Loaded += OnLoaded;
                element.Unloaded -= OnUnloaded;
                element.Unloaded += OnUnloaded;
                if (element.IsLoaded)
                    Subscribe(element);
            }
        }

        private static readonly DependencyProperty HandlerProperty =
            DependencyProperty.RegisterAttached("Handler", typeof(Action), typeof(LanguageExtension));

        private static void Subscribe(FrameworkElement element)
        {
            if (element.GetValue(HandlerProperty) is Action)
                return;

            Action handler = () => element.Dispatcher.Invoke(() => UpdateText(element));
            element.SetValue(HandlerProperty, handler);
            LanguageManager.LanguageChanged += handler;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            Subscribe(element);
            UpdateText(element);
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (element.GetValue(HandlerProperty) is Action handler)
            {
                LanguageManager.LanguageChanged -= handler;
                element.ClearValue(HandlerProperty);
            }
        }

        private static void UpdateText(DependencyObject element)
        {
            string key = GetText(element);

            if (string.IsNullOrEmpty(key))
                return;

            string translatedText = LanguageManager.GetString(key);

            if (element is TextBlock textBlock)
            {
                textBlock.Text = translatedText;
            }
            else if (element is Button button)
            {
                button.Content = translatedText;
            }
        }
    }
}
