﻿using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class BitOptionEditorViewModel : ViewModelBase, IBitOptionEditorViewModel
    {
        private IBitOption _model;
        private List<int> _numbersOfAssotiatedBits;

        public string FullSugnature { get; private set; }

        public List<int> NumbersOfAssotiatedBits
        {
            get { return this._numbersOfAssotiatedBits; }
            set
            {
                this._numbersOfAssotiatedBits = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.IsEnabled));
            }
        }

        public bool IsEnabled => !(this.NumbersOfAssotiatedBits.Count > 0 && !this._model.VariableColumnSignature.IsMultipleAssignmentAllowed);
        public void UpdateIsEnabled()
        {
            this.RaisePropertyChanged(nameof(this.IsEnabled));
        }

        public string StrongName => nameof(BitOptionEditorViewModel);

        public object Model
        {
            get
            {
                this._model.NumbersOfAssotiatedBits = this.NumbersOfAssotiatedBits;
                return this._model;
            }
            set
            {

                this._model = value as IBitOption;
                this.FullSugnature = this._model.FullSignature;
                this.NumbersOfAssotiatedBits = this._model.NumbersOfAssotiatedBits;
                this.RaisePropertyChanged(nameof(this.FullSugnature));
            }
        }

        public override string ToString()
        {
            return FullSugnature;
        }
    }
}
