using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace kRPClient.Displays
{
    /// <summary>
    /// Interaction logic for ResourceDisplay.xaml
    /// </summary>
    public partial class ResourceDisplay : UserControl
    {
        public static readonly DependencyProperty ResourceItemsProperty =
            DependencyProperty.Register("ResourceItems", typeof(string), typeof(ResourceDisplay), new PropertyMetadata(OnItemsSourceChanged));

        public ResourceDisplay()
        {
            InitializeComponent();
        }

        public string ResourceItems
        {
            get
            {
                return (string)GetValue(ResourceItemsProperty);
            }
            set
            {
                SetValue(ResourceItemsProperty, value);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == DataContextProperty)
            {
                Binding myBinding = new Binding(string.Format("Resources.{0}", ResourceItems));
                myBinding.Source = DataContext;
                DisplayGrid.SetBinding(DataGrid.ItemsSourceProperty, myBinding);
            }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourceDisplay resourcesDisplay = (ResourceDisplay)d;
            resourcesDisplay.ResourceItems = (string)e.NewValue;
        }
    }
}