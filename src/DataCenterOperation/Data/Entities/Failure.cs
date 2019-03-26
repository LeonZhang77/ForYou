using System;
using System.ComponentModel.DataAnnotations;

namespace DataCenterOperation.Data.Entities
{
    public class Failure : AbstractEntity
    {
        [StringLength(50)]
        public string DeviceId { get; set; }

        [StringLength(100)]
        public string DeviceName { get; set; }

        [StringLength(200)]
        public string DeviceLocation { get; set; }

        [StringLength(50)]
        public string WhoRecorded { get; set; }

        public DateTime? WhenRecorded { get; set; }

        [StringLength(1000)]
        public string FailureCause { get; set; }

        public DateTime? WhenReported { get; set; }

        /// <summary>
        /// Telephone or Email.
        /// </summary>
        [StringLength(50)]
        public string WayReportedVia { get; set; }

        /// <summary>
        /// Company or Engineer.
        /// </summary>
        [StringLength(50)]
        public string TargetReportedTo { get; set; }

        [StringLength(50)]
        public string TargetEngineerName { get; set; }

        public bool? HasReportedToSpecifiedPerson { get; set; }

        [StringLength(1000)]
        public string CommentsFromSpecifiedPerson { get; set; }

        public bool? HasServiceReportSubmitted { get; set; }

        [StringLength(1000)]
        public string WhyNoServiceReportSubmitted { get; set; }

        [StringLength(1000)]
        public string Solution { get; set; }

        [StringLength(50)]
        public string WhoSolved { get; set; }

        public DateTime? WhenSolved { get; set; }

        [StringLength(1000)]
        public string SuperiorComments { get; set; }

        [StringLength(50)]
        public string SuperiorSignature { get; set; }

        public DateTime? WhenSigned { get; set; }
    }
}
