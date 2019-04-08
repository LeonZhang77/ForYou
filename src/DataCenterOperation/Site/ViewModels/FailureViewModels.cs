using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataCenterOperation.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [StringLength(50)]
        [Display(Name = "故障上报的方式")]
        public string WayReportedVia { get; set; }

        /// <summary>
        /// Company or Engineer.
        /// </summary>
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

        public Guid Id { get; set; }

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

    public class FailureEditViewModel : FailureCreateViewModel
    {
        public static FailureEditViewModel CreateFromEntity(Failure failure)
        {
            return new FailureEditViewModel
            {
                Id = failure.Id,
                DeviceId = failure.DeviceId,
                DeviceName = failure.DeviceName,
                DeviceLocation = failure.DeviceLocation,
                WhoRecorded = failure.WhoRecorded,
                DateRecorded = failure.WhenRecorded,
                TimeRecorded = failure.WhenRecorded,
                FailureCause = failure.FailureCause,
                DateReported = failure.WhenReported,
                TimeReported = failure.WhenReported,
                WayReportedVia = failure.WayReportedVia,
                TargetReportedTo = failure.TargetReportedTo,
                TargetEngineerName = failure.TargetEngineerName,
                HasReportedToSpecifiedPerson = failure.HasReportedToSpecifiedPerson ?? false,
                CommentsFromSpecifiedPerson = failure.CommentsFromSpecifiedPerson,
                HasServiceReportSubmitted = failure.HasServiceReportSubmitted ?? false,
                ServiceReportId = failure.ServiceReportId,
                WhyNoServiceReportSubmitted = failure.WhyNoServiceReportSubmitted,
                Solution = failure.Solution,
                SolutionEngineer = failure.WhoSolved,
                DateSolved = failure.WhenSolved,
                TimeSolved = failure.WhenSolved,
                SuperiorComments = failure.SuperiorComments,
                SuperiorSignature = failure.SuperiorSignature,
                SuperiorSignatureDate = failure.WhenSigned,
            };
        }
    }

    public class FailureDetailsViewModel
    {
        [Display(Name = "设备序列号")]
        public string DeviceId { get; set; }

        [Display(Name = "设备名称")]
        public string DeviceName { get; set; }

        [Display(Name = "设备所在的物理位置")]
        public string DeviceLocation { get; set; }

        [Display(Name = "登记人")]
        public string WhoRecorded { get; set; }

        [Display(Name = "登记时间")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? WhenRecorded { get; set; }

        [Display(Name = "故障原因")]
        public string FailureCause { get; set; }

        [Display(Name = "故障上报的时间")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? WhenReported { get; set; }

        [Display(Name = "故障上报的方式")]
        public string WayReportedVia { get; set; }

        [Display(Name = "故障上报的对象")]
        public string TargetReportedTo { get; set; }

        [Display(Name = "故障上报的工程师姓名")]
        public string TargetEngineerName { get; set; }

        [Display(Name = "是否已经上报")]
        public string HasReportedToSpecifiedPerson { get; set; }

        [Display(Name = "处理意见或建议")]
        public string CommentsFromSpecifiedPerson { get; set; }

        [Display(Name = "是否提交服务报告")]
        public string HasServiceReportSubmitted { get; set; }

        [Display(Name = "故障报告编号")]
        public string ServiceReportId { get; set; }

        [Display(Name = "没有提交报告的原因说明")]
        public string WhyNoServiceReportSubmitted { get; set; }

        [Display(Name = "故障的解决办法")]
        public string Solution { get; set; }

        [Display(Name = "故障解决工程师姓名")]
        public string SolutionEngineer { get; set; }

        [Display(Name = "故障解决时间")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? WhenSolved { get; set; }

        [Display(Name = "信息中心领导建议及评价")]
        public string SuperiorComments { get; set; }

        [Display(Name = "领导签名")]
        public string SuperiorSignature { get; set; }

        [Display(Name = "领导签名日期")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SuperiorSignatureDate { get; set; }

        public Guid Id { get; set; }

        public static FailureDetailsViewModel CreateFromEntity(Failure failure)
        {
            return new FailureDetailsViewModel
            {
                Id = failure.Id,
                DeviceId = failure.DeviceId,
                DeviceName = failure.DeviceName,
                DeviceLocation = failure.DeviceLocation,
                WhoRecorded = failure.WhoRecorded,
                WhenRecorded = failure.WhenRecorded,
                FailureCause = failure.FailureCause,
                WhenReported = failure.WhenReported,
                WayReportedVia = failure.WayReportedVia,
                TargetReportedTo = failure.TargetReportedTo,
                TargetEngineerName = failure.TargetEngineerName,
                HasReportedToSpecifiedPerson = !failure.HasReportedToSpecifiedPerson.HasValue ? string.Empty : (failure.HasReportedToSpecifiedPerson.Value ? "是" : "否"),
                CommentsFromSpecifiedPerson = failure.CommentsFromSpecifiedPerson,
                HasServiceReportSubmitted = !failure.HasServiceReportSubmitted.HasValue ? string.Empty : (failure.HasServiceReportSubmitted.Value ? "是" : "否"),
                ServiceReportId = failure.ServiceReportId,
                WhyNoServiceReportSubmitted = failure.WhyNoServiceReportSubmitted,
                Solution = failure.Solution,
                SolutionEngineer = failure.WhoSolved,
                WhenSolved = failure.WhenSolved,
                SuperiorComments = failure.SuperiorComments,
                SuperiorSignature = failure.SuperiorSignature,
                SuperiorSignatureDate = failure.WhenSigned,
            };
        }
    }

    public class FailureSearchViewModel
    {
        [Display(Prompt = "关键字")]
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int Count { get; set; }
        public IEnumerable<FailureListItem> Failures { get; set; }

        public bool HasPrevPage { get { return PageIndex > 1; } }
        public bool HasNextPage { get { return PageIndex < TotalPages; } }
        public int TotalPages { get { return (int)Math.Ceiling(Count / (double)PageSize); } }
    }

    public class FailureListItem
    {
        [Display(Name = "序号")]
        public int Index { get; set; }

        [Display(Name = "设备名称")]
        public string DeviceName { get; set; }

        [Display(Name = "故障原因")]
        public string FailureCause { get; set; }

        [Display(Name = "发现日期")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? DateRecorded { get; set; }

        [Display(Name = "解决日期")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? DateSolved { get; set; }

        [Display(Name = "记录人")]
        public string WhoRecorded { get; set; }

        public Guid Id { get; set; }
    }
}
