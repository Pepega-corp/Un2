using System;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.View;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class AppointableMatrixEditorViewModel : EditorConfigurationItemViewModelBase, IAppointableMatrixEditorViewModel
    {
        public AppointableMatrixEditorViewModel(IMatrixTemplateEditorViewModel matrixTemplateEditorViewModel)
        {
            MatrixTemplateEditorViewModel = matrixTemplateEditorViewModel;
        }

        public override string TypeName => StrongName;
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitMatrix(this);
        }

        public virtual string StrongName => ConfigurationKeys.APPOINTABLE_MATRIX;

        public bool IsInEditMode { get; set; }
        public void StartEditElement()
        {
            AppointableMatrixEditorWindow appointableMatrixEditorWindow = new AppointableMatrixEditorWindow();
            appointableMatrixEditorWindow.DataContext = MatrixTemplateEditorViewModel;
            appointableMatrixEditorWindow.ShowDialog();
            //this._model.Name = this.MatrixTemplateEditorViewModel.MatrixName;
            //this.Header = this._model.Name;
        }

        public void StopEditElement()
        {

        }

        public void DeleteElement()
        {
            if (Parent != null)
            {
                if (Parent is IChildItemRemovable)
                {
                 //   (this.Parent as IChildItemRemovable).RemoveChildItem((this._model as IProperty));
                }
            }
        }

        public IMatrixTemplateEditorViewModel MatrixTemplateEditorViewModel { get; }

        //protected override void SetModel(object model)
        //{
        //    IAppointableMatrix matrix = model as IAppointableMatrix;
        //    this.MatrixTemplateEditorViewModel.Model = matrix.MatrixTemplate;
        //    this.MatrixTemplateEditorViewModel.MatrixName = matrix.Name;
        //    base.SetModel(model);
        //}
    }
}
