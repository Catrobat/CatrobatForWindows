﻿using Windows.UI.ViewManagement;
using Catrobat.IDE.Core.Resources.Localization;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Editor.Formula;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Catrobat.IDE.WindowsPhone.Controls.FormulaControls;

namespace Catrobat.IDE.WindowsPhone.Views.Editor.Formula
{
    public partial class FormulaEditorView
    {
        private readonly FormulaEditorViewModel _viewModel = 
            ServiceLocator.ViewModelLocator.FormulaEditorViewModel;

        

        public FormulaEditorView()
        {
            InitializeComponent();

            //ButtonAddLocalVariable = (ApplicationBarIconButton) ApplicationBar.Buttons[2];
            //ButtonAddGlobalVariable = (ApplicationBarIconButton) ApplicationBar.Buttons[3];

            //IsAddLocalVariableButtonVisible = _viewModel.IsAddLocalVariableButtonVisible;
            //IsAddGlobalVariableButtonVisible = _viewModel.IsAddGlobalVariableButtonVisible;
            
            this.FormulaViewerMain.DoubleTap += FormulaViewer_DoubleTap;
            this.FormulaKeyboard.KeyPressed += KeyPressed;

            Loaded += FormulaEditorView_OnLoaded;
            Unloaded += FormulaEditorView_OnUnloaded;
            Window.Current.SizeChanged += CurrentOnSizeChanged;
        }

        private void CurrentOnSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Task 589, code taken from source: http://msdn.microsoft.com/en-us/library/windows/apps/dn495655.aspx
            // Get the new view state
            // Add: using Windows.UI.ViewManagement;
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            // Trigger the Visual State Manager
            VisualStateManager.GoToState(this, CurrentViewState, true);
        
        }

        private void FormulaEditorView_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.FormulaViewerMain.SetBinding(FormulaViewer.TokensProperty, new Binding { Path = new PropertyPath("Tokens"), Mode = BindingMode.OneWay });
            this.FormulaViewerMain.SetBinding(FormulaViewer.CaretIndexProperty, new Binding { Path = new PropertyPath("CaretIndex"), Mode = BindingMode.TwoWay });
            this.FormulaViewerMain.SetBinding(FormulaViewer.SelectionStartProperty, new Binding { Path = new PropertyPath("SelectionStart"), Mode = BindingMode.TwoWay });
            this.FormulaViewerMain.SetBinding(FormulaViewer.SelectionLengthProperty, new Binding { Path = new PropertyPath("SelectionLength"), Mode = BindingMode.TwoWay });
            this.FormulaKeyboard.SetBinding(Controls.FormulaControls.FormulaKeyboard.CanDeleteProperty, new Binding { Path = new PropertyPath("CanDelete") });
            this.FormulaKeyboard.SetBinding(Controls.FormulaControls.FormulaKeyboard.CanEvaluateProperty, new Binding { Path = new PropertyPath("CanEvaluate") });
            this.FormulaKeyboard.SetBinding(Controls.FormulaControls.FormulaKeyboard.HasErrorProperty, new Binding { Path = new PropertyPath("HasError") });
            this.FormulaKeyboard.SetBinding(Controls.FormulaControls.FormulaKeyboard.ProgramProperty, new Binding { Path = new PropertyPath("CurrentProgram") });
            
            //_viewModel.PropertyChanged += ViewModel_OnPropertyChanged;
        }

        private void FormulaEditorView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.FormulaViewerMain.ClearValue(FormulaViewer.TokensProperty);
            this.FormulaViewerMain.ClearValue(FormulaViewer.CaretIndexProperty);
            this.FormulaViewerMain.ClearValue(FormulaViewer.SelectionStartProperty);
            this.FormulaViewerMain.ClearValue(FormulaViewer.SelectionLengthProperty);
            this.FormulaKeyboard.ClearValue(Controls.FormulaControls.FormulaKeyboard.CanDeleteProperty);
            this.FormulaKeyboard.ClearValue(Controls.FormulaControls.FormulaKeyboard.CanEvaluateProperty);
            this.FormulaKeyboard.ClearValue(Controls.FormulaControls.FormulaKeyboard.HasErrorProperty);
            this.FormulaKeyboard.ClearValue(Controls.FormulaControls.FormulaKeyboard.ProgramProperty);

            //_viewModel.PropertyChanged -= ViewModel_OnPropertyChanged;
        }

        //private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsAddLocalVariableButtonVisible") IsAddLocalVariableButtonVisible = _viewModel.IsAddLocalVariableButtonVisible;
        //    if (e.PropertyName == "IsAddGlobalVariableButtonVisible") IsAddGlobalVariableButtonVisible = _viewModel.IsAddGlobalVariableButtonVisible;
        //}

        #region Dependency properties

        //public static readonly DependencyProperty IsAddLocalVariableButtonVisibleProperty = DependencyProperty.Register(
        //    name: "IsAddLocalVariableButtonVisible",
        //    propertyType: typeof (bool),
        //    ownerType: typeof (FormulaEditorView),
        //    typeMetadata: new PropertyMetadata(true, (d, e) => ((FormulaEditorView) d).IsAddLocalVariableButtonVisiblePropertyChanged(e)));
        //public bool IsAddLocalVariableButtonVisible
        //{
        //    get { return (bool)GetValue(IsAddLocalVariableButtonVisibleProperty); }
        //    set { SetValue(IsAddLocalVariableButtonVisibleProperty, value); }
        //}
        //private void IsAddLocalVariableButtonVisiblePropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    if (((bool)e.NewValue))
        //    {
        //        ApplicationBar.Buttons.Add(ButtonAddLocalVariable);
        //    }
        //    else
        //    {
        //        ApplicationBar.Buttons.Remove(ButtonAddLocalVariable);
        //    }
        //}

        //public static readonly DependencyProperty IsAddGlobalVariableButtonVisibleProperty = DependencyProperty.Register
        //    (
        //        name: "IsAddGlobalVariableButtonVisible",
        //        propertyType: typeof (bool),
        //        ownerType: typeof (FormulaEditorView),
        //        typeMetadata: new PropertyMetadata(true, (d, e) => ((FormulaEditorView) d).IsAddGlobalVariableButtonVisiblePropertyChanged(e)));
        //public bool IsAddGlobalVariableButtonVisible
        //{
        //    get { return (bool)GetValue(IsAddGlobalVariableButtonVisibleProperty); }
        //    set { SetValue(IsAddGlobalVariableButtonVisibleProperty, value); }
        //}
        //private void IsAddGlobalVariableButtonVisiblePropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    if (((bool)e.NewValue))
        //    {
        //        ApplicationBar.Buttons.Add(ButtonAddGlobalVariable);
        //    }
        //    else
        //    {
        //        ApplicationBar.Buttons.Remove(ButtonAddGlobalVariable);
        //    }
        //}

        #endregion

        #region Transition animations

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var animation = this.FormulaKeyboard.Resources["EnterTransition"] as Storyboard;
            if (animation != null)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                animation.Begin();
            }
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var animation = this.FormulaKeyboard.Resources["ExitTransition"] as Storyboard;
            if (animation != null)
            {
                animation.Begin();
                await Task.Delay(TimeSpan.FromSeconds(0.3));
            }
            base.OnNavigatingFrom(e);
        }

        #endregion

        private void FormulaViewer_DoubleTap(int index)
        {
            _viewModel.CompleteTokenCommand.Execute(index);
        }

        private void KeyPressed(FormulaKeyEventArgs e)
        {
            _viewModel.KeyPressedCommand.Execute(e);
        }

        private bool _firstBackPressed = true;
        //protected override void OnBackKeyPress(CancelEventArgs e)
        //{
        //    if (_viewModel.HasError && _firstBackPressed)
        //    {
        //        e.Cancel = true;
        //        _firstBackPressed = false;

        //        var timeToAct = TimeSpan.FromSeconds(1);
        //        ServiceLocator.NotifictionService.ShowToastNotification(
        //            title: "",
        //            message: AppResourcesHelper.Get("Editor_ReallyDismissFormula,
        //            timeTillHide: timeToAct);
        //        Task.Run(async () =>
        //        {
        //            await Task.Delay(timeToAct);
        //            _firstBackPressed = true;
        //        });
        //    }
        //    else
        //    {
        //        _viewModel.GoBackCommand.Execute(null);
        //        e.Cancel = true;
        //        base.OnBackKeyPress(e);
        //    }
        //}
    }
}
