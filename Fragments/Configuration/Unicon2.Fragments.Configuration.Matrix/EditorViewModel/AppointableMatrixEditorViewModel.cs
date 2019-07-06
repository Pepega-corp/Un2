using System;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.View;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class AppointableMatrixEditorViewModel : EditorConfigurationItemViewModelBase, IAppointableMatrixEditorViewModel
    {
        public AppointableMatrixEditorViewModel(IMatrixTemplateEditorViewModel matrixTemplateEditorViewModel)
        {
            this.MatrixTemplateEditorViewModel = matrixTemplateEditorViewModel;
        }
        
        #region Overrides of ConfigurationItemViewModelBase

        public override string TypeName => this.StrongName;
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override string StrongName => ConfigurationKeys.APPOINTABLE_MATRIX;

        #endregion

        #region Implementation of IEditable

        public bool IsInEditMode { get; set; }
        public void StartEditElement()
        {
            AppointableMatrixEditorWindow appointableMatrixEditorWindow = new AppointableMatrixEditorWindow();
            appointableMatrixEditorWindow.DataContext = this.MatrixTemplateEditorViewModel;
            appointableMatrixEditorWindow.ShowDialog();
            this._model.Name = this.MatrixTemplateEditorViewModel.MatrixName;
            this.Header = this._model.Name;
        }

        public void StopEditElement()
        {

        }

        #endregion

        #region Implementation of IDeletable

        public void DeleteElement()
        {
            if (this.Parent != null)
            {
                if (this.Parent is IChildItemRemovable)
                {
                    (this.Parent as IChildItemRemovable).RemoveChildItem((this._model as IProperty));
                }
            }
        }

        #endregion

        #region Implementation of IAppointableMatrixEditorViewModel

        public IMatrixTemplateEditorViewModel MatrixTemplateEditorViewModel { get; }

        #endregion
        
        #region Overrides of ConfigurationItemViewModelBase

        protected override void SetModel(object model)
        {
            IAppointableMatrix matrix = model as IAppointableMatrix;
            this.MatrixTemplateEditorViewModel.Model = matrix.MatrixTemplate;
            this.MatrixTemplateEditorViewModel.MatrixName = matrix.Name;
            base.SetModel(model);
        }

        #endregion
    }
}
