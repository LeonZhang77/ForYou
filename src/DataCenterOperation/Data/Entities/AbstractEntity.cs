using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public abstract class AbstractAssertEntity : LoggableEntity
    {

        [StringLength(50)]
        public string FixedAssertNumber { get; set; } //配置项编号

        [StringLength(50)]
        public string Name { get; set; } //名称

        [StringLength(50)]
        public string SerialNumber { get; set; } //序列号

        [StringLength(50)]
        public string ContractNumber { get; set; } //合同编号
    }
    public abstract class AbstractEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
