using System;

namespace Unicon.Backend.WebApi.Entities
{
    public class CommandsStoreEntry
    {
        public int Id { get; set; }
        public int SequenceNumber { get; set; }
        public int RelatedCommandId { get; set; }
        public string CommandType { get; set; }
        public DateTime CommandDateTime { get; set; }
    }
}