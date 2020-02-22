using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace FocusMaster.Controls
{
    [TemplatePart(Name = "BackgroundBorderElement", Type = typeof(Border))]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Hovered", GroupName = "HoveredStates")]
    [TemplateVisualState(Name = "Unhovered", GroupName = "HoveredStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "PressedStates")]
    [TemplateVisualState(Name = "Unpressed", GroupName = "PressedStates")]
    [TemplateVisualState(Name = "Selected", GroupName = "SelectedStates")]
    [TemplateVisualState(Name = "Unselected", GroupName = "SelectedStates")]
    public class ImageButton : ToggleButton
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BackgroundBorder = GetTemplateChild("BackgroundBorder") as Border;
            ForegroundBorder = GetTemplateChild("ForegroundBorder") as Border;

            UpdateStates(true);
        }

        public static readonly DependencyProperty BackgroundBorderProperty =
            DependencyProperty.Register(
                "BackgroundBorder",
                typeof(Border),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ForegroundBorderProperty =
            DependencyProperty.Register(
                "ForegroundBorder",
                typeof(Border),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender
                    | FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        public static readonly DependencyProperty ToggleableProperty =
            DependencyProperty.Register(
                "Toggleable",
                typeof(bool),
                typeof(ImageButton),
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
