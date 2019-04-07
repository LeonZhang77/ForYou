using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using DataCenterOperation.Data.Entities;
using Newtonsoft.Json;
using System.IO;


namespace DataCenterOperation.Site.ViewModels
{
    public class AssertX86ServerVerifyListViewModel
    {
           public AssertX86ServerVerifyListViewModel() 
           {
               this.ReadyToAdd = new List<AssertX86ServerViewModel>();
               this.ReadyToModify = new List<AssertX86ServerViewModel>();
               this.ErrorItems = new List<AssertX86ServerViewModel>();
           }
           public List<AssertX86ServerViewModel> ReadyToAdd { get; set; }
           public List<AssertX86ServerViewModel> ReadyToModify { get; set; }
           public List<AssertX86ServerViewModel> ErrorItems {get; set; }
    }

    public class AssertX86ServerViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "配置项编号")]
        public string FixedAssertNumber { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "序列号")]
        public string SerialNumber { get; set; }

        //硬盘
        [Display(Name = "硬盘")]
        public string HD { get; set; }
        //操作系统
        [Display(Name = "操作系统")]
        public string OS { get; set; }
        //机房编号
        [Display(Name = "机房编号")]
        public string EngineNumber { get; set; }
        //机柜位置
        [Display(Name = "机柜位置")]
        public string RackLocation { get; set; }
        //起始U位
        [Display(Name = "起始U位")]
        public string BeginU { get; set; }
        //结束U位
        [Display(Name = "结束U位")]
        public string EndU { get; set; }
        //虚拟化资源池
        [Display(Name = "虚拟化资源池")]
        public string VirtualizedResourcePool { get; set; }
        //业务系统
        [Display(Name = "业务系统")]
        public string BusinessSystem { get; set; }
        //IP地址
        [Display(Name = "IP地址")]
        public string IP { get; set; }
        //网卡数量
        [Display(Name = "网卡数量")]
        public string NetcardNumber { get; set; }
        //HBA卡数量
        [Display(Name = "HBA卡数量")]
        public string HBANumber { get; set; }
        //存储空间
        [Display(Name = "存储空间")]
        public string StorageSize { get; set; }
        //维保信息
        [Display(Name = "维保信息")]
        
        public string MaintenanceInformation { get; set; }
        //安装日期
        [Display(Name = "安装日期")]
        [DataType(DataType.Date)]
        public DateTime? InstallDate { get; set; }
        //品牌型号
        [Display(Name = "品牌型号")]
        public string Band { get; set; }
        //CPU
        [Display(Name = "CPU")]
        public string CPU { get; set; }
        //内存
        [Display(Name = "内存")]
        public string Memory { get; set; }
    }
}
