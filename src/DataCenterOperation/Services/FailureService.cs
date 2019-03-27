using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Site.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataCenterOperation.Services
{
    public interface IFailureService
    {
        Task<Failure> RecordFailureAsync(FailureCreateViewModel model);
    }

    public class FailureService : IFailureService
    {
        private DataCenterOperationDbContext _dbContext;

        private ILogger<FailureService> _logger;

        public FailureService(DataCenterOperationDbContext dbContext, ILogger<FailureService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Failure> RecordFailureAsync(FailureCreateViewModel model)
        {
            var failure = new Failure
            {
                Id = new Guid(),
                DeviceId = model.DeviceId,
                DeviceName = model.DeviceName,
                DeviceLocation = model.DeviceLocation,
                WhoRecorded = model.WhoRecorded,
                WhenRecorded = CombineDateAndTime(model.DateRecorded, model.TimeRecorded),
                FailureCause = model.FailureCause,
                WhenReported = CombineDateAndTime(model.DateReported, model.TimeReported),
                WayReportedVia = model.WayReportedVia,
                TargetReportedTo = model.TargetReportedTo,
                TargetEngineerName = model.TargetEngineerName,
                HasReportedToSpecifiedPerson = model.HasReportedToSpecifiedPerson,
                CommentsFromSpecifiedPerson = model.CommentsFromSpecifiedPerson,
                HasServiceReportSubmitted = model.HasServiceReportSubmitted,
                WhyNoServiceReportSubmitted = model.WhyNoServiceReportSubmitted,
                Solution = model.Solution,
                WhoSolved = model.SolutionEngineer,
                WhenSolved = CombineDateAndTime(model.DateSolved, model.TimeSolved),
                SuperiorComments = model.SuperiorComments,
                SuperiorSignature = model.SuperiorSignature,
                WhenSigned = model.SuperiorSignatureDate
            };

            _dbContext.Failures.Add(failure);
            await _dbContext.SaveChangesAsync();

            return failure;
        }

        private DateTime? CombineDateAndTime(DateTime? date, DateTime? time)
        {
            if (!date.HasValue || !time.HasValue) return null;

            return new DateTime(date.Value.Year,
                date.Value.Month,
                date.Value.Day,
                time.Value.Hour,
                time.Value.Minute,
                0);
        }
    }
}
