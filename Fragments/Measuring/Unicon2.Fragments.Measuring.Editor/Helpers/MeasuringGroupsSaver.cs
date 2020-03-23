using System.Runtime.Remoting.Messaging;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
    public class MeasuringGroupsSaver
    {
        public IMeasuringGroup CreateMeasuringGroup(IMeasuringGroupEditorViewModel measuringGroupEditorViewModel)
        {
            var res = StaticContainer.Container.Resolve<IMeasuringGroup>();
            res.PresentationSettings = StaticContainer.Container.Resolve<IPresentationSettings>();
            res.Name = measuringGroupEditorViewModel.Header;
            res.MeasuringElements.Clear();
            var saver = new MeasuringElementSaver();
            foreach (IMeasuringElementEditorViewModel measuringElementEditorViewModel in
                measuringGroupEditorViewModel.MeasuringElementEditorViewModels)
            {
                res.MeasuringElements.Add(saver.SaveMeasuringElement(measuringElementEditorViewModel));
            }

            return res;
        }
    }
}