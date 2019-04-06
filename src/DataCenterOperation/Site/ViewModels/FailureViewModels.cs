using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Site.ViewModels
{
    public class FailureCreateViewModel
    {
        [StringLength(50)]
        [Display(Name = "设备序列号")]
        public string DeviceId { get; set; }

        [StringLength(100)]
        [Display(Name = "设备名称")]
        public string DeviceName { get; set; }

        [StringLength(200)]
        [Display(Name = "设备所在的物理位置")]
        public string DeviceLocation { get; set; }

        [StringLength(50)]
        [Display(Name = "登记人")]
        public string WhoRecorded { get; set; }

        [Display(Name = "登记时间")]
        [DataType(DataType.Date)]
        public DateTime? DateRecorded { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeRecorded { get; set; }

        [StringLength(1000)]
        [Display(Name = "故障原因")]
        [DataType(DataType.MultilineText)]
        public string FailureCause { get; set; }

        [Display(Name = "故障上报的时间")]
        [DataType(DataType.Date)]
        public DateTime? DateReported { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeReported { get; set; }

        /// <summary>
        /// Telephone or Email.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "故障上报的方式")]
        public string WayReportedVia { get; set; }

        /// <summary>
        /// Company or Engineer.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "故障上报的对象")]
        public string TargetReportedTo { get; set; }

        [StringLength(50)]
        [Display(Name = "故障上报的工程师姓名")]
        public string TargetEngineerName { get; set; }

        [Display(Name = "是否已经上报")]
        public bool HasReportedToSpecifiedPerson { get; set; }

        [StringLength(1000)]
        [Display(Name = "是否有处理意见或建议")]
        [DataType(DataType.MultilineText)]
        public string CommentsFromSpecifiedPerson { get; set; }

        [Display(Name = "是否提交服务报告")]
        public bool HasServiceReportSubmitted { get; set; }

        [StringLength(50)]
        [Display(Name = "故障报告编号")]
        public string ServiceReportId { get; set; }

        [StringLength(1000)]
        [Display(Name = "没有提交报告的原因情况说明")]
        [DataType(DataType.MultilineText)]
        public string WhyNoServiceReportSubmitted { get; set; }

        [StringLength(1000)]
        [Display(Name = "故障的解决办法")]
        [DataType(DataType.MultilineText)]
        public string Solution { get; set; }

        [StringLength(50)]
        [Display(Name = "故障解决工程师姓名")]
        public string SolutionEngineer { get; set; }

        [Display(Name = "故障解决时间")]
        [DataType(DataType.Date)]
        public DateTime? DateSolved { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeSolved { get; set; }

        [StringLength(1000)]
        [Display(Name = "信息中心领导建议及评价")]
        [DataType(DataType.MultilineText)]
        public string SuperiorComments { get; set; }

        [StringLength(50)]
        [Display(Name = "领导签名")]
        public string SuperiorSignature { get; set; }

        [Display(Name = "领导签名日期")]
        [DataType(DataType.Date)]
        public DateTime? SuperiorSignatureDate { get; set; }

        public List<SelectListItem> ReportWays { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "电话", Text = "电话" },
            new SelectListItem { Value = "邮件", Text = "邮件" }
        };

        public List<SelectListItem> ReportTargets { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "公司", Text = "公司" },
            new SelectListItem { Value = "工程师", Text = "工程师" }
        };
    }
}
