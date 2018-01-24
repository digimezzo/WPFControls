using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Digimezzo.WPFControls
{
    public enum DateFormat
    {
        DayMonthYear = 0,
        MonthDayYear = 1,
        YearMonthDay = 2,
        YearDayMonth = 3
    }

    public class MaterialDateBox : Control, IDisposable
    {
        MaterialComboBox boxDay;
        MaterialComboBox boxMonth;
        MaterialComboBox boxYear;
        private TextBlock errorLabel;
        private StackPanel panel;
        private bool disposedValue = false;

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(MaterialDateBox), new PropertyMetadata(true));

        public int Day
        {
            get { return (int)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public static readonly DependencyProperty DayProperty =
            DependencyProperty.Register(nameof(Day), typeof(int), typeof(MaterialDateBox), new PropertyMetadata(0));

        public int Month
        {
            get { return (int)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }

        public static readonly DependencyProperty MonthProperty =
            DependencyProperty.Register(nameof(Month), typeof(int), typeof(MaterialDateBox), new PropertyMetadata(0));

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register(nameof(Year), typeof(int), typeof(MaterialDateBox), new PropertyMetadata(0));

        public ValidationMode ValidationMode
        {
            get { return (ValidationMode)GetValue(ValidationModeProperty); }
            set { SetValue(ValidationModeProperty, value); }
        }

        public static readonly DependencyProperty ValidationModeProperty =
            DependencyProperty.Register(nameof(ValidationMode), typeof(ValidationMode), typeof(MaterialDateBox), new PropertyMetadata(ValidationMode.None));


        public DateFormat DateFormat
        {
            get { return (DateFormat)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        public Brush Accent
        {
            get { return (Brush)GetValue(AccentProperty); }
            set { SetValue(AccentProperty, value); }
        }

        public static readonly DependencyProperty AccentProperty =
            DependencyProperty.Register(nameof(Accent), typeof(Brush), typeof(MaterialDateBox), new PropertyMetadata(Brushes.Blue));

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingProperty); }
            set { SetValue(IsFloatingProperty, value); }
        }

        public static readonly DependencyProperty IsFloatingProperty =
            DependencyProperty.Register(nameof(IsFloating), typeof(bool), typeof(MaterialDateBox), new PropertyMetadata(false));

        public string LabelDay
        {
            get { return (string)GetValue(LabelDayProperty); }
            set { SetValue(LabelDayProperty, value); }
        }

        public static readonly DependencyProperty LabelDayProperty =
            DependencyProperty.Register(nameof(LabelDay), typeof(string), typeof(MaterialDateBox), new PropertyMetadata("Day", new PropertyChangedCallback(OnLabelDayPropertyChanged)));

        private static void OnLabelDayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialDateBox dateBox = d as MaterialDateBox;

            if (dateBox != null && dateBox.boxDay != null && dateBox.boxDay.InputLabel != null)
            {
                dateBox.boxDay.InputLabel.Text = dateBox.boxDay.Label;
            }
        }

        public string LabelMonth
        {
            get { return (string)GetValue(LabelMonthProperty); }
            set { SetValue(LabelMonthProperty, value); }
        }

        public static readonly DependencyProperty LabelMonthProperty =
            DependencyProperty.Register(nameof(LabelMonth), typeof(string), typeof(MaterialDateBox), new PropertyMetadata("Month", new PropertyChangedCallback(OnLabelMonthPropertyChanged)));

        private static void OnLabelMonthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialDateBox dateBox = d as MaterialDateBox;

            if (dateBox != null && dateBox.boxMonth != null && dateBox.boxMonth.InputLabel != null)
            {
                dateBox.boxMonth.InputLabel.Text = dateBox.boxMonth.Label;
            }
        }

        public string LabelYear
        {
            get { return (string)GetValue(LabelYearProperty); }
            set { SetValue(LabelYearProperty, value); }
        }

        public static readonly DependencyProperty LabelYearProperty =
            DependencyProperty.Register(nameof(LabelYear), typeof(string), typeof(MaterialDateBox), new PropertyMetadata("Year", new PropertyChangedCallback(OnLabelYearPropertyChanged)));

        private static void OnLabelYearPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialDateBox dateBox = d as MaterialDateBox;

            if (dateBox != null && dateBox.boxYear != null && dateBox.boxYear.InputLabel != null)
            {
                dateBox.boxYear.InputLabel.Text = dateBox.boxYear.Label;
            }
        }

        public Brush ErrorForeground
        {
            get { return (Brush)GetValue(ErrorForegroundProperty); }
            set { SetValue(ErrorForegroundProperty, value); }
        }

        public static readonly DependencyProperty ErrorForegroundProperty =
            DependencyProperty.Register(nameof(ErrorForeground), typeof(Brush), typeof(MaterialDateBox), new PropertyMetadata(Brushes.Red));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(MaterialDateBox), new PropertyMetadata("Invalid"));

        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register(nameof(DateFormat), typeof(DateFormat), typeof(MaterialDateBox), new PropertyMetadata(DateFormat.DayMonthYear));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.errorLabel = (TextBlock)GetTemplateChild("PART_ErrorLabel");
            this.panel = (StackPanel)GetTemplateChild("PART_Panel");

            switch (this.DateFormat)
            {
                case DateFormat.DayMonthYear:
                    this.boxDay = (MaterialComboBox)GetTemplateChild("PART_Box1");
                    this.boxMonth = (MaterialComboBox)GetTemplateChild("PART_Box2");
                    this.boxYear = (MaterialComboBox)GetTemplateChild("PART_Box3");
                    break;
                case DateFormat.MonthDayYear:
                    this.boxMonth = (MaterialComboBox)GetTemplateChild("PART_Box1");
                    this.boxDay = (MaterialComboBox)GetTemplateChild("PART_Box2");
                    this.boxYear = (MaterialComboBox)GetTemplateChild("PART_Box3");
                    break;
                case DateFormat.YearMonthDay:
                    this.boxYear = (MaterialComboBox)GetTemplateChild("PART_Box1");
                    this.boxMonth = (MaterialComboBox)GetTemplateChild("PART_Box2");
                    this.boxDay = (MaterialComboBox)GetTemplateChild("PART_Box3");
                    break;
                case DateFormat.YearDayMonth:
                    this.boxYear = (MaterialComboBox)GetTemplateChild("PART_Box1");
                    this.boxDay = (MaterialComboBox)GetTemplateChild("PART_Box2");
                    this.boxMonth = (MaterialComboBox)GetTemplateChild("PART_Box3");
                    break;
                default:
                    break;
            }

            this.errorLabel.FontSize = this.GetSmallFontSize();
            this.errorLabel.Margin = this.ValidationMode.Equals(ValidationMode.None) ? new Thickness(0) : new Thickness(0, this.GetMargin(), 0, 0);
            this.panel.Margin = this.IsFloating ? new Thickness(0, this.GetSmallFontSize() + this.GetMargin(), 0, 0) : new Thickness(0);

            this.boxDay.Label = this.LabelDay;
            this.boxMonth.Label = this.LabelMonth;
            this.boxYear.Label = this.LabelYear;

            this.boxDay.ItemsSource = this.Days();
            this.boxMonth.ItemsSource = this.Months();
            this.boxYear.ItemsSource = this.Years();

            this.boxDay.SelectionChanged += BoxDay_SelectionChanged;
            this.boxMonth.SelectionChanged += BoxMonth_SelectionChanged;
            this.boxYear.SelectionChanged += BoxYear_SelectionChanged;
        }

        private void BoxDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is MaterialComboBox)
            {
                MaterialComboBox box = (MaterialComboBox)sender;

                if (box.SelectedItem is int)
                {
                    this.Day = (int)box.SelectedItem;
                    this.Validate();
                }
            }
        }

        private void BoxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is MaterialComboBox)
            {
                MaterialComboBox box = (MaterialComboBox)sender;

                if(box.SelectedItem is int)
                {
                    this.Month = (int)box.SelectedItem;
                    this.Validate();
                }
            }
        }

        private void BoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is MaterialComboBox)
            {
                MaterialComboBox box = (MaterialComboBox)sender;

                if (box.SelectedItem is int)
                {
                    this.Year = (int)box.SelectedItem;
                    this.Validate();
                }
            }
        }

        static MaterialDateBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialDateBox), new FrameworkPropertyMetadata(typeof(MaterialDateBox)));
        }

        private double GetSmallFontSize()
        {
            return this.FontSize > 14 ? this.FontSize * 0.7 : 10;
        }

        private double GetMargin()
        {
            return this.FontSize * 0.3;
        }

        public List<int> Days()
        {
            var days = new List<int>();

            for (int i = 1; i <= 31; i++)
            {
                days.Add(i);
            }

            return days;
        }

        public List<int> Months()
        {
            var days = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                days.Add(i);
            }

            return days;
        }

        public List<int> Years()
        {
            var years = new List<int>();

            for (int i = 1979; i <= DateTime.Now.Year; i++)
            {
                years.Add(i);
            }

            return years;
        }

        private void Validate()
        {
            if (!this.ValidationMode.Equals(ValidationMode.Date))
            {
                return;
            }

            if (this.Day.Equals(0) || this.Month.Equals(0) || this.Year.Equals(0))
            {
                return;
            }

            DateTime dateValue;

            if (DateTime.TryParseExact($"{this.Year}-{this.Month.ToString("D2")}-{this.Day.ToString("D2")}", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
            {
                this.errorLabel.Text = String.Empty;
                this.IsValid = true;
            }
            else
            {
                this.errorLabel.Text = this.ErrorText;
                this.IsValid = false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.boxDay != null)
                    {
                        this.boxDay.SelectionChanged -= BoxDay_SelectionChanged;
                    }

                    if (this.boxMonth != null)
                    {
                        this.boxMonth.SelectionChanged -= BoxMonth_SelectionChanged;
                    }

                    if (this.boxYear != null)
                    {
                        this.boxYear.SelectionChanged -= BoxYear_SelectionChanged;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
