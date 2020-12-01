using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class VariableSignatureEditorViewModel : ViewModelBase, IVariableSignatureEditorViewModel
    {
        private IVariableColumnSignature _model;
        private string _signature;
        private bool _isMultipleAssgnmentAllowed;

        public string StrongName => nameof(VariableSignatureEditorViewModel);

        public object Model
        {
            get
            {
                _model.Signature = Signature;
                _model.IsMultipleAssignmentAllowed = IsMultipleAssgnmentAllowed;
                return _model;

            }
            set
            {
                _model = value as IVariableColumnSignature;
                Signature = _model.Signature;
                IsMultipleAssgnmentAllowed = _model.IsMultipleAssignmentAllowed;
            }
        }

        public string Signature
        {
            get { return _signature; }
            set
            {
                _signature = value;
                RaisePropertyChanged();
            }
        }

        public bool IsMultipleAssgnmentAllowed
        {
            get { return _isMultipleAssgnmentAllowed; }
            set
            {
                _isMultipleAssgnmentAllowed = value;
                RaisePropertyChanged();
            }
        }
    }
}
