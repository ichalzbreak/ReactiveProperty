﻿using System;
using Microsoft.Phone.Reactive;
using System.Windows;
using Codeplex.Reactive; // using Namespace

namespace WP7.ViewModels
{
    // ReactiveProperty and ReactiveCommand simple example.
    public class ReactivePropertyBasicsViewModel
    {
        public ReactiveProperty<string> InputText { get; private set; }
        public ReactiveProperty<string> DisplayText { get; private set; }
        public ReactiveCommand ReplaceTextCommand { get; private set; }

        public ReactivePropertyBasicsViewModel()
        {
            // mode is Flags.
            var allMode = ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe;

            // binding value from UI Control
            InputText = new ReactiveProperty<string>(initialValue: "", mode: allMode);

            // send value to UI Control
            DisplayText = InputText
                .Select(s => s.ToUpper())       // rx query1
                .Delay(TimeSpan.FromSeconds(1)) // rx query2
                .ToReactiveProperty();          // convert to ReactiveProperty

            ReplaceTextCommand = InputText
                .Select(s => !string.IsNullOrEmpty(s))   // condition sequence of CanExecute
                .ToReactiveCommand(); // convert to ReactiveCommand

            // ReactiveCommand's Subscribe is set ICommand's Execute
            // ReactiveProperty.Value set is push(& set) value
            ReplaceTextCommand.Subscribe(_ => InputText.Value = "Hello, ReactiveProperty!");
        }
    }
}