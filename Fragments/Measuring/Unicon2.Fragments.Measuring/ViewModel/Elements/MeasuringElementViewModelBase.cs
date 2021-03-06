﻿using System;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public abstract class MeasuringElementViewModelBase : ValidatableBindableBase, IMeasuringElementViewModel
    {
        private IFormattedValueViewModel _formattedValueViewModel;
        private string _groupName;


        public abstract string StrongName { get; }

        public string Header { get; set; }

        public string GroupName
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                RaisePropertyChanged();
            }
        }

        public IFormattedValueViewModel FormattedValueViewModel
        {
            get { return _formattedValueViewModel; }
            set
            {
                _formattedValueViewModel = value;
                RaisePropertyChanged();
            }
        }

  


        public Guid Id { get; set; }

        public void SetId(Guid id)
        {
	        Id = id;
        }
    }
}
