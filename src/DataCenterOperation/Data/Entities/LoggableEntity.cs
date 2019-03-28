using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    /// <summary>
    /// If your class needs the fields defined here, 
    /// then make it derive from this class.
    /// </summary>
    public abstract class LoggableEntity : AbstractEntity
    {
        public DateTime? CreatedTime { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }
    }
}
