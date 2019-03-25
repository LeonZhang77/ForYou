using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class VistorEntourage : AbstractEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
       
        [StringLength(50)]
        public string Identity { get; set; }        

        [StringLength(50)]
        public string Company { get; set; }

        public Guid VistorEntryRequestGuid { get; set; }
    }
}
