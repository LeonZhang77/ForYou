using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class VistorRecord : AbstractEntity
    {
        [StringLength(50)]
        public string VistorName { get; set; }
                
        public int NumberOfPeople { get; set; }

        public DateTime EntryTime { get; set; }

        [StringLength(255)]
        public string Company { get; set; }

        [StringLength(255)]
        public string Matter { get; set; }

        [StringLength(255)]
        public string ContactInfo { get; set; }

        public Guid VistorEntryRequestGuid { get; set; }
       
    }
}
