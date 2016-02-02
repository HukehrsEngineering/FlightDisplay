﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace kRPClient.Displays
{
    /// <summary>
    /// Interaction logic for ResourceDisplay.xaml
    /// </summary>
    public partial class ResourceDisplay : UserControl
    {
        // Using a DependencyProperty as the backing store for MyHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyHeaderProperty =
            DependencyProperty.Register("MyHeader", typeof(string), typeof(ResourcesDisplay), new PropertyMetadata(default(string), new PropertyChangedCallback(OnLabelChanged)));

        // Using a DependencyProperty as the backing store for ResourcesShown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResourcesShownProperty =
            DependencyProperty.Register("ResourcesShown", typeof(string), typeof(ResourcesDisplay), new PropertyMetadata(default(string), new PropertyChangedCallback(OnResourcesShownChanged)));

        private bool hasInit = false;

        public ResourceDisplay()
        {
            InitializeComponent();
        }

        public string MyHeader
        {
            get { return (string)GetValue(MyHeaderProperty); }
            set { SetValue(MyHeaderProperty, value); HeaderLabel.Content = value; }
        }

        public string ResourcesShown
        {
            get { return (string)GetValue(ResourcesShownProperty); }
            set
            {
                SetValue(ResourcesShownProperty, value);
                //BuildResourceDisplay();
            }
        }

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourcesDisplay resourcesDisplay = (ResourcesDisplay)d;
            resourcesDisplay.Content = (string)e.NewValue;
        }

        private static void OnResourcesShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ResourcesDisplay resourcesDisplay = (ResourcesDisplay)d;
            resourcesDisplay.ResourcesShown = (string)e.NewValue;
        }

        private void AddToGridAt(UIElement element, int row, int column)
        {
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            ResourceGrid.Children.Add(element);
        }

        private void BuildResourceDisplay()
        {
            if (!hasInit && !string.IsNullOrWhiteSpace(ResourcesShown))
            {
                var resourcesToDisplay = ResourcesShown.Split(new char[] { ',' });

                GridLength rowHeight = new GridLength(32);
                int currentRow = 1;
                //ResourceGrid.Height = 32 * (resourcesToDisplay.Length + 1);
                //ResourceGrid.ShowGridLines = true;
                var overviewModel = ((MainWindow)((Grid)this.Parent).Parent).DataContext;

                foreach (var resource in resourcesToDisplay)
                {
                    RowDefinition row = new RowDefinition() { Height = rowHeight };
                    ResourceGrid.RowDefinitions.Add(row);

                    Label name = new Label() { Content = resource };
                    Binding nameBinding = new Binding(string.Format("Resources.Resources[\"{0}\"].Name", resource));
                    nameBinding.Source = this;
                    name.SetBinding(Label.ContentProperty, nameBinding);
                    AddToGridAt(name, currentRow, 0);

                    Binding maximumBinding = new Binding(string.Format("Resources.Resources[\"{0}\"].Maximum", resource));
                    maximumBinding.Source = this;
                    Binding valueBinding = new Binding(string.Format("Resources.Resources[\"{0}\"].Ammount", resource));
                    valueBinding.Source = this;

                    ProgressBar bar = new ProgressBar()
                    {
                        Minimum = 0
                    };
                    bar.SetBinding(ProgressBar.MaximumProperty, maximumBinding);
                    bar.SetBinding(ProgressBar.ValueProperty, valueBinding);
                    AddToGridAt(bar, currentRow, 1);

                    Label ammountLabel = new Label();
                    ammountLabel.SetBinding(Label.ContentProperty, valueBinding);
                    ammountLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    AddToGridAt(ammountLabel, currentRow, 2);

                    Label slashLabel = new Label() { Content = "/" };
                    AddToGridAt(slashLabel, currentRow, 3);

                    Label maximumLabel = new Label();
                    maximumLabel.SetBinding(Label.ContentProperty, maximumBinding);
                    AddToGridAt(maximumLabel, currentRow, 4);

                    currentRow++;
                }
                ResourceGrid.InvalidateVisual();
                ResourceGrid.UpdateLayout();
                this.InvalidateVisual();
                this.UpdateLayout();
                hasInit = true;
            }
        }
    }
}