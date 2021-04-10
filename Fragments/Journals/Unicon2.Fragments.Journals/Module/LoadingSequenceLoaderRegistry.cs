using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers;
using Unicon2.Fragments.Journals.MemoryAccess;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Journals.Module
{
   public class LoadingSequenceLoaderRegistry: ILoadingSequenceLoaderRegistry
   {
       private Dictionary<Type, Func<DeviceContext, IJournalLoadingSequence, ISequenceLoader>> _loaders=new Dictionary<Type, Func<DeviceContext, IJournalLoadingSequence, ISequenceLoader>>();
       

        public void AddLoader<T>(Func<DeviceContext, IJournalLoadingSequence, ISequenceLoader> loader)
        {
            _loaders.AddElement(typeof(T), loader);
        }

        public ISequenceLoader ResolveLoader(IJournalLoadingSequence loadingSequence, DeviceContext deviceContext)
        {
            return _loaders[loadingSequence.GetType()](deviceContext, loadingSequence);
        }
   }
}
