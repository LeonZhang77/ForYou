using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using DataCenterOperation.Data.Entities;
using Newtonsoft.Json;
using System.IO;

namespace DataCenterOperation.Site.ViewModels
{
    public class EntryViewModel
    {

        public Guid ID { get; set; }

        [Display(Name = "访客名")]
        public string VistorName { get; set; }

        [Display(Name = "人数")]
        public int NumberOfPeople { get; set; }

        [Display(Name = "日期及时间")]
        public DateTime EntryTime { get; set; }
        
        [Display(Name = "所属单位")]
        public string Company { get; set; }

        [Display(Name = "来访事由")]
        public string Matter { get; set; }

        [Display(Name = "来访人联系电话")]
        public string ContactInfo { get; set; }

        public Guid VistorEntryRequestGuid { get; set; }

    }

    public class EntryRequestViewModel
    {

        [Display(Name = "申请人")]
        public string RequestPeoopleName { get; set; }

        [Display(Name = "所属单位")]
        public string Company { get; set; }

        [Display(Name = "申请时间")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "进入时间")]
        public DateTime BeginTime { get; set; }

        [Display(Name = "离开时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "携带物品")]
        public string Belongings { get; set; }

        [Display(Name = "事由")]
        public string Matter_Short { get; set; }

        [Display(Name = "工作事项说明")]
        public string Matter_Details { get; set; }

        [Display(Name = "进入区域")]
        public string Area { get; set; }

        [Display(Name = "机房管理员审核")]
        public string Admin_Confirm { get; set; }
        [Display(Name = "数据中心分管领导审批")]
        public string Manager_Confirm { get; set; }

        public string Entourage { get; set; }

        public static ICollection<VistorEntourage> GetEntourage(string jsonString) {
            List<VistorEntourage> result = new List<VistorEntourage>();
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(jsonString);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<VistorEntourageClass>));
            List<VistorEntourageClass> vistorEntourageClass = o as List<VistorEntourageClass>;
            foreach (VistorEntourageClass item in vistorEntourageClass)
            {
                VistorEntourage vistorEntourage = new VistorEntourage
                {
                    Name = item.Name,
                    Identity = item.Identity,
                    Company = item.Company,                    
                };
                result.Add(vistorEntourage);
            }
            return result;
        }

        private class VistorEntourageClass {
            public string Name { get; set; }
            public string Identity { get; set; }
            public string Company { get; set; }
        }
    }
}
