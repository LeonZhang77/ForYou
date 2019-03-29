using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class Assert_X86Server: AbstractAssertEntity
    {
        //硬盘
        [StringLength(50)]
        public string HD { get; set; }
        //操作系统
        [StringLength(50)]
        public string OS { get; set; }
        //机房编号
        [StringLength(50)]
        public string EngineNumber { get; set; }
        //机柜位置
        [StringLength(50)]
        public string RackLocation { get; set; }
        //起始U位
        [StringLength(50)]
        public string BeginU { get; set; }
        //结束U位
        [StringLength(50)]
        public string EndU { get; set; }
        //虚拟化资源池
        [StringLength(50)]
        public string VirtualizedResourcePool { get; set; }
        //业务系统
        [StringLength(50)]
        public string BusinessSystem { get; set; }
        //IP地址
        [StringLength(50)]
        public string IP { get; set; }
        //网卡数量
        [StringLength(50)]
        public string NetcardNumber { get; set; }
        //HBA卡数量
        [StringLength(50)]
        public string HBANumber { get; set; }
        //存储空间
        [StringLength(50)]
        public string StorageSize { get; set; }
        //维保信息
        [StringLength(50)]
        public string MaintenanceInformation { get; set; }
        //安装日期        
        public DateTime? InstallDate { get; set; }
        //品牌型号
        [StringLength(50)]
        public string Band { get; set; }
        //CPU
        [StringLength(50)]
        public string CPU { get; set; }
        //内存
        [StringLength(50)]
        public string Memory { get; set; }      

    }
}
