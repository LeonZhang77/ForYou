using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class Assert_X86ServerUserInformation : AbstractAssertEntity
    {
        //用户名
        [StringLength(50)]
        public string UserName { get; set; }
        //用户描述
        [StringLength(50)]
        public string UserDescription { get; set; }
        //责任人
        [StringLength(50)]
        public string PersonInCharge { get; set; }
    }
}
