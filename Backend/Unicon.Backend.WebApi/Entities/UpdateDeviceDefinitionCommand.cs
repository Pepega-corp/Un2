using System.Collections.Generic;

namespace Unicon.Backend.WebApi.Entities
{
    public class UpdateDeviceDefinitionCommand
    {
        public int Id { get; set; }
        public string DefinitionString { get; set; }
        public string TagsOneLine { get; set; }
    }
}