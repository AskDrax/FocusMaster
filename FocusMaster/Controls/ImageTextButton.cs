using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace FocusMaster.Controls
{
    [TemplatePart(Name = "BackgroundBorderElement", Type = typeof(Border))]
    [TemplatePart(Name = "IconBorderElement", Type = typeof(Border))]
    [TemplatePart(Name = "TextBorderElement", Type = typeof(Border))]
    [TemplatePart(Name = "IconPartElement", Type = typeof(Image))]
    [TemplatePart(Name = "TextPartElement", Type = typeof(TextBlock))]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Hovered", GroupName = "HoveredStates")]
    [TemplateVisualState(Name = "Unhovered", GroupName = "HoveredStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "PressedStates")]
    [TemplateVisualState(Name = "Unpressed", GroupName = "PressedStates")]
    [TemplateVisualState(Name = "Selected", GroupName = "SelectedStates")]
    [TemplateVisualState(Name = "Unselected", GroupName = "SelectedStates")]
    public class ImageTextButton : ToggleButton
    {
        static ImageTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextButton),
                new FrameworkPropertyMetadata(typeof(ImageTextButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BackgroundBorder = GetTemplateChild("BackgroundBorder") as Border;
            ForegroundBorder = GetTemplateChild("ForegroundBorder") as Border;
            IconBorder = GetTemplateChild("IconBorder") as Border;
            TextBorder = GetTemplateChild("TextBorder") as Border;
            IconPart = GetTemplateChild("IconPart") as Image;
            TextPart = GetTemplateChild("TextPart") as TextBlock;

            //IconAreaWidth = new GridLength(64, GridUnitType.Pixel);
            //TextAreaWidth = new GridLength(1, GridUnitType.Star);

            UpdateStates(true);
        }

        public static readonly DependencyProperty IconAreaWidthProperty =
            DependencyProperty.Register(
                "IconAreaWidth",
                typeof(GridLength),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(new GridLength(64, GridUnitType.Pixel),
                    FrameworkPropertyMetadataOptions.AffectsArrange
                    | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty TextAreaWidthProperty =
            DependencyProperty.Register(
                "TextAreaWidth",
                typeof(GridLength),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star),
                    FrameworkPropertyMetadataOptions.AffectsArrange
                    | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty BackgroundBorderProperty =
            DependencyProperty.Register(
                "BackgroundBorder",
                typeof(Border),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ForegroundBorderProperty =
            DependencyProperty.Register(
                "ForegroundBorder",
                typeof(Border),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender
                    | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty IconBGColorProperty =
            DependencyProperty.Register(
                "IconBGColor",
                typeof(Brush),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextBGColorProperty =
            DependencyProperty.Register(
                "TextBGColor",
                typeof(Brush),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextFGColorProperty =
            DependencyProperty.Register(
                "TextFGColor",
                typeof(Brush),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IconBorderProperty =
            DependencyProperty.Register(
                "IconBorder",
                typeof(Border),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextBorderProperty =
            DependencyProperty.Register(
                "TextBorder",
                typeof(Border),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ForegroundImageProperty =
            DependencyProperty.Register(
                "ForegroundImage",
                typeof(ImageBrush),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender
                    | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty IconPartProperty =
            DependencyProperty.Register(
                "IconPart",
                typeof(Image),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ImageTextButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TextPartProperty =
            DependencyProperty.Register(
                "TextPart",
                typeof(TextBlock),
                typeof(ImageTextButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ToggleableProperty =
            DependencyProperty.Register(
                "Toggleable",
                typeof(bool),
                typeof(ImageTextButton),
                new PropertyMetadata(true));

        public Border BackgroundBorder
        {
            get { return (Border)GetValue(BackgroundBorderProperty); }
            set { SetValue(BackgroundBorderProperty, value); }
        }

        public Border ForegroundBorder
        {
            get { return (Border)GetValue(ForegroundBorderProperty); }
            set { SetValue(ForegroundBorderProperty, value); }
        }

        public Border IconBorder
        {
            get { return (Border)GetValue(IconBorderProperty); }
            set { SetValue(IconBorderProperty, value); }
        }

        public Brush IconBGColor
        {
            get { return (Brush)GetValue(IconBGColorProperty); }
            set { SetValue(IconBGColorProperty, value); }
        }

        public Brush TextBGColor
        {
            get { return (Brush)GetValue(TextBGColorProperty); }
            set { SetValue(TextBGColorProperty, value); }
        }

        public Brush TextFGColor
        {
            get { return (Brush)GetValue(TextFGColorProperty); }
            set { SetValue(TextFGColorProperty, value); }
        }

        public Border TextBorder
        {
            get { return (Border)GetValue(TextBorderProperty); }
            set { SetValue(TextBorderProperty, value); }
        }

        public ImageBrush ForegroundImage
        {
            get { return (ImageBrush)GetValue(ForegroundImageProperty); }
            set { SetValue(ForegroundImageProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public TextBlock TextPart
        {
            get { return (TextBlock)GetValue(TextPartProperty); }
            set { SetValue(TextPartProperty, value); }
        }

        public Image IconPart
        {
            get { return (Image)GetValue(IconPartProperty); }
            set { SetValue(IconPartProperty, value); }
        }


        public GridLength IconAreaWidth
        {
            get { return (GridLength)GetValue(IconAreaWidthProperty); }
            set { SetValue(IconAreaWidthProperty, value); }
        }

        public GridLength TextAreaWidth
        {
            get { return (GridLength)GetValue(TextAreaWidthProperty); }
            set { SetValue(TextAreaWidthProperty, value); }
        }

        public bool Toggleable
        {
            get { return (bool)GetValue(ToggleableProperty); }
            set { SetValue(ToggleableProperty, value); }
        }

        private void UpdateStates(bool useTransitions)
        {
            if (IsFocused)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }

            if (IsMouseOver)
            {
                //if (IsChecked == false)
                    VisualStateManager.GoToState(this, "Hovered", useTransitions);
            }
            else
            {
                if (Toggleable == false || IsChecked == false)
                    VisualStateManager.GoToState(this, "Unhovered", useTransitions);
            }

            if (IsPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unpressed", useTransitions);
            }

            if (Toggleable == true)
            {
                if (IsChecked == true)
                {
                    VisualStateManager.GoToState(this, "Pressed", useTransitions);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Unpressed", useTransitions);
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateStates(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateStates(true);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            UpdateStates(true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            UpdateStates(true);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            UpdateStates(true);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (IsChecked == false)
                IsChecked = true;
            UpdateStates(true);
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            UpdateStates(true);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            UpdateStates(true);
        }
    }
}
