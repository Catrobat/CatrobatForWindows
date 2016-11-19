﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Catrobat.IDE.Core.Annotations;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Models.Bricks;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.UI;
using Catrobat.IDE.Core.UI.Converters;
using Catrobat.IDE.Core.Utilities.Helpers;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Editor.Formula;

namespace Catrobat.IDE.WindowsPhone.Controls.FormulaControls
{
    public partial class VariableButton : UserControl, INotifyPropertyChanged
    {
        #region DependencyProperties

        public Variable Variable
        {
            get { return (Variable) GetValue(VariableProperty); }
            set { SetValue(VariableProperty, value); }
        }

        public static readonly DependencyProperty VariableProperty = 
            DependencyProperty.Register("Variable", typeof(Variable), 
            typeof(VariableButton), new PropertyMetadata(null, VariableChanged));

        private static void VariableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VariableButton)d).RaisePropertyChanged(() => ((VariableButton)d).Variable);
            ((VariableButton)d).VariableChanged((Variable) e.NewValue);
        }

        #endregion

        private new static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VariableButton)d).IsEnabled = (bool)e.NewValue;
        }

        public void VariableChanged(Variable newVariable)
        {
            var setVariableBrick = DataContext as SetVariableBrick;
            if (setVariableBrick != null)
                setVariableBrick.Variable = newVariable;

            var changeVariableBrick = DataContext as ChangeVariableBrick;
            if (changeVariableBrick != null)
                changeVariableBrick.Variable = newVariable;

            var isSelected = newVariable != null;
            var converter = new NullVariableConverter();

            newVariable = (Variable) converter.Convert(newVariable, null, null, null);

            var viewModel = ((ViewModelLocator)ServiceLocator.ViewModelLocator).VariableSelectionViewModel;

            TextBlockVariableName.Text = newVariable.Name;

            //if (isSelected)
            //{
            //    TextBlockVariableName.Foreground = VariableHelper.IsVariableLocal(viewModel.CurrentProgram, newVariable) ?
            //        new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Gray);
            //}
            //else
            //{
            //    TextBlockVariableName.Foreground = new SolidColorBrush(Colors.Red);
            //}
        }

        public VariableButton()
        {
            InitializeComponent();
            VariableChanged(null);
        }

        private void ButtonFormula_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = ((ViewModelLocator)ServiceLocator.ViewModelLocator).VariableSelectionViewModel;

            var container = new VariableConteiner {Variable = Variable};
            container.PropertyChanged += ContainerOnPropertyChanged;
            viewModel.SelectedVariableContainer = container;

            ServiceLocator.NavigationService.NavigateTo<VariableSelectionViewModel>();
        }

        private void ContainerOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var container = (VariableConteiner)sender;

            if (args.PropertyName == PropertyHelper.
                GetPropertyName(() => container.Variable))
            {
                VariableChanged(container.Variable);
                //SetValue(VariableProperty, container.Variable);
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> selector)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyHelper.GetPropertyName(selector)));
            }
        }

        #endregion
    }
}
