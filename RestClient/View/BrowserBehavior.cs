using System.Windows;
using System.Windows.Controls;

namespace RestClient.View
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(BrowserBehavior),
            new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser browser)
        {
            return browser.GetValue(HtmlProperty).ToString();
        }

        public static void SetHtml(WebBrowser browser, string value)
        {
            browser.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = obj as WebBrowser;
            if (browser != null)
            {
                browser.NavigateToString(e.NewValue as string);
            }
        }
    }
}
