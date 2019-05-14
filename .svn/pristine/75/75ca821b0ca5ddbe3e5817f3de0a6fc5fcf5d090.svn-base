using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class LogicElementViewModel : ViewModelBase, ILogicElementViewModel
    {
        protected LogicElementViewModel(string strongName)
        {
            this.StrongName = strongName;
            this.DebugMode = false;
            this.IsSelected = false;
            this.ValidationError = false;
        }

        private bool _isSelected;
        private ILogicElement _model;
        private bool _debugMode;
        private string _caption;
        private bool _validationError;
        private string _description;

        public string ElementName { get; protected set; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.RaisePropertyChanged();
            }
        }

        public string StrongName { get; }

        public object Model
        {
            get { return this._model; }
            set { this._model = value as ILogicElement; }
        }

        public string Symbol { get; protected set; }

        public string Caption
        {
            get { return this._caption; }
            set
            {
                this._caption = value;
                this.RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return this._description; }
            protected set
            {
                this._description = value;
                this.RaisePropertyChanged();
            }
        }
        public bool ValidationError
        {
            get { return this._validationError; }
            set
            {
                this._validationError = value;
                this.RaisePropertyChanged();
            }
        }
        public bool DebugMode
        {
            get { return this._debugMode; }
            set
            {
                this._debugMode = value;
                this.RaisePropertyChanged();
            }
        }
        public virtual object Clone()
        {
            LogicElementViewModel ret = new LogicElementViewModel(this.StrongName);
            ret.Model = (this.Model as ILogicElement)?.Clone();
            ret.IsSelected = this.IsSelected;
            ret.DebugMode = this.DebugMode;
            ret.Caption = this.Caption;
            ret.ValidationError = this.ValidationError;
            return ret;
        }
    }
}
