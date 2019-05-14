using System.Collections.Specialized;
using System.Windows;
using MahApps.Metro.Controls;
using Prism.Regions;

namespace Unicon2.Shell.ControlRegionAdapter
{

    public class FlyoutsControlRegionAdapter : RegionAdapterBase<FlyoutsControl>
    {
        public FlyoutsControlRegionAdapter(IRegionBehaviorFactory factory)
            : base(factory)
        {
        }

        protected override void Adapt(IRegion region, FlyoutsControl regionTarget)
        {
            region.ActiveViews.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (FrameworkElement element in e.NewItems)
                    {
                        if (element != null)
                            regionTarget.Items.Add(element);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}