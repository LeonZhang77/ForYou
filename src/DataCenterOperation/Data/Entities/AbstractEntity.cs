using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public abstract class AbstractEntity : LoggableEntity
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class LoggableEntity
    {
        public DateTime? CreatedTime { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }
    }
}
