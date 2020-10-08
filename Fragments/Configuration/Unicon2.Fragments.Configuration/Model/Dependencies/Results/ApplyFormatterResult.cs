using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Dependencies.Results
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApplyFormatterResult : IApplyFormatterResult
    {
        [JsonProperty] public string Name { get; set; }

        public object Clone()
        {
            return new ApplyFormatterResult()
            {
                Name = Name,
                UshortsFormatter = this.UshortsFormatter.Clone() as IUshortsFormatter
            };
        }

        [JsonProperty] public IUshortsFormatter UshortsFormatter { get; set; }
    }
}