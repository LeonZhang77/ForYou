using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public abstract class AbstractEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
