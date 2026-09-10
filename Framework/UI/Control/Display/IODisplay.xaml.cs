using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UI.Control.Display
{
    /// <summary>
    /// IODisplay.xaml 的交互逻辑
    /// </summary>
    public partial class IODisplay : UserControl
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(IEnumerable), typeof(IODisplay));

        public IEnumerable? Items
        {
            get => (IEnumerable?)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public IODisplay()
        {
            InitializeComponent();
        }
    }
}
