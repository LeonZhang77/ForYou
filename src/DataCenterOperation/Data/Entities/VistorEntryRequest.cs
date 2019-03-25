using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class VistorEntryRequest : AbstractEntity
    {
        [StringLength(50)]
        public string RequestPeoopleName { get; set; }
        [StringLength(255)]
        public string Company { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        [StringLength(50)]
        public string Area { get; set; }
        [StringLength(50)]
        public string Belongings { get; set; }
        [StringLength(50)]
        public string Matter_Short { get; set; }
        [StringLength(255)]
        public string Matter_Details { get; set; }

        [StringLength(255)]
        public string Admin_Confirm { get; set; }
        [StringLength(255)]
        public string Manager_Confirm { get; set; }
        
        public ICollection<VistorEntourage> Entourage { get; set; }

    }
}
