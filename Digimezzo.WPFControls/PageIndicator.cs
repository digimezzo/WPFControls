using Digimezzo.WPFControls.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public class PageIndicatorItem : PropertyChangedBase
    {
        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.isActive = value;
                OnPropertyChanged(nameof(this.IsActive));
            }
        }
    }

    public class PageIndicator : Control
    {
        public double ItemSize
        {
            get { return (double)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register(nameof(ItemSize), typeof(double), typeof(PageIndicator), new PropertyMetadata(10.0));

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }

        public static readonly DependencyProperty ItemSpacingProperty =
            DependencyProperty.Register(nameof(ItemSpacing), typeof(double), typeof(PageIndicator), new PropertyMetadata(4.0));

        [Browsable(false)]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(PageIndicator), new PropertyMetadata(new Thickness(2, 0, 2, 0)));

        public Brush ItemFill
        {
            get { return (Brush)GetValue(ItemFillProperty); }
            set { SetValue(ItemFillProperty, value); }
        }

        public static readonly DependencyProperty ItemFillProperty =
            DependencyProperty.Register(nameof(ItemFill), typeof(Brush), typeof(PageIndicator), new PropertyMetadata(Brushes.Gray));

        public Brush SelectedItemFill
        {
            get { return (Brush)GetValue(SelectedItemFillProperty); }
            set { SetValue(SelectedItemFillProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemFillProperty =
            DependencyProperty.Register(nameof(SelectedItemFill), typeof(Brush), typeof(PageIndicator), new PropertyMetadata(Brushes.Red));

        public int NumberOfItems
        {
            get { return (int)GetValue(NumberOfElementsProperty); }
            set { SetValue(NumberOfElementsProperty, value); }
        }

        public static readonly DependencyProperty NumberOfElementsProperty =
            DependencyProperty.Register(nameof(NumberOfItems), typeof(int), typeof(PageIndicator), new PropertyMetadata(3));

        public int ActiveItem
        {
            get { return (int)GetValue(ActiveItemProperty); }
            set { SetValue(ActiveItemProperty, value); }
        }

        public static readonly DependencyProperty ActiveItemProperty =
            DependencyProperty.Register(nameof(ActiveItem), typeof(int), typeof(PageIndicator), new PropertyMetadata(0, new PropertyChangedCallback(OnActiveItemPropertyChanged)));

        private static void OnActiveItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageIndicator indicator = d as PageIndicator;

            if (indicator != null)
            {
                indicator.SetActiveItem();
            }
        }

        static PageIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageIndicator), new FrameworkPropertyMetadata(typeof(PageIndicator)));
        }

        public ObservableCollection<PageIndicatorItem> Items { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.ItemMargin = new Thickness(this.ItemSpacing / 2, 0, this.ItemSpacing / 2, 0);

            this.Items = new ObservableCollection<PageIndicatorItem>();

            for (int i = 0; i < this.NumberOfItems; i++)
            {
                this.Items.Add(new PageIndicatorItem());

                if (i == 0)
                {
                    this.Items[0].IsActive = true;
                }
            }
        }

        public void SetActiveItem()
        {
            if (this.Items != null && this.Items.Count > 0 && this.ActiveItem >= 0 && this.ActiveItem < this.Items.Count)
            {
                foreach (PageIndicatorItem item in this.Items)
                {
                    item.IsActive = false;
                }

                this.Items[this.ActiveItem].IsActive = true;
            }
        }
    }
}
