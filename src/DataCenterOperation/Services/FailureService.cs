using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Site.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Services
{
    public interface IFailureService
    {
        Task<Failure> RecordFailureAsync(FailureCreateViewModel model);
        Task<Failure> UpdateAsync(FailureEditViewModel model);
        Task DeleteAsync(Guid id);
        Task<Failure> GetAsync(Guid id);
        Task<List<Failure>> GetAsync(string keyword, int pageIndex = 1, int pageSize = 20);
        Task<int> CountAsync(string keyword);
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

        public async Task<int> CountAsync(string keyword)
        {
            var query = PrepareQuery(keyword);

            return await query.CountAsync();
        }

        public async Task<List<Failure>> GetAsync(string keyword, int pageIndex, int pageSize)
        {
            var query = PrepareQuery(keyword);

            return await query.OrderByDescending(f => f.WhenRecorded)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        private IQueryable<Failure> PrepareQuery(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? from f in _dbContext.Failures select f
                : from f in _dbContext.Failures
                  where f.DeviceId.Contains(keyword)
                  || f.DeviceName.Contains(keyword)
                  || f.DeviceLocation.Contains(keyword)
                  select f;
        }

        public async Task<Failure> GetAsync(Guid id)
        {
            return await _dbContext.Failures.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var failure = await _dbContext.Failures.FirstOrDefaultAsync(f => f.Id == id);

            if (failure == null) return;

            _dbContext.Failures.Remove(failure);
            await _dbContext.SaveChangesAsync();
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
                ServiceReportId = model.ServiceReportId,
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

        public async Task<Failure> UpdateAsync(FailureEditViewModel model)
        {
            var failure = await _dbContext.Failures.FirstOrDefaultAsync(f => f.Id == model.Id);

            failure.DeviceId = model.DeviceId;
            failure.DeviceName = model.DeviceName;
            failure.DeviceLocation = model.DeviceLocation;
            failure.WhoRecorded = model.WhoRecorded;
            failure.WhenRecorded = CombineDateAndTime(model.DateRecorded, model.TimeRecorded);
            failure.FailureCause = model.FailureCause;
            failure.WhenReported = CombineDateAndTime(model.DateReported, model.TimeReported);
            failure.WayReportedVia = model.WayReportedVia;
            failure.TargetReportedTo = model.TargetReportedTo;
            failure.TargetEngineerName = model.TargetEngineerName;
            failure.HasReportedToSpecifiedPerson = model.HasReportedToSpecifiedPerson;
            failure.CommentsFromSpecifiedPerson = model.CommentsFromSpecifiedPerson;
            failure.HasServiceReportSubmitted = model.HasServiceReportSubmitted;
            failure.ServiceReportId = model.ServiceReportId;
            failure.WhyNoServiceReportSubmitted = model.WhyNoServiceReportSubmitted;
            failure.Solution = model.Solution;
            failure.WhoSolved = model.SolutionEngineer;
            failure.WhenSolved = CombineDateAndTime(model.DateSolved, model.TimeSolved);
            failure.SuperiorComments = model.SuperiorComments;
            failure.SuperiorSignature = model.SuperiorSignature;
            failure.WhenSigned = model.SuperiorSignatureDate;

            await _dbContext.SaveChangesAsync();

            return failure;
        }

        private DateTime? CombineDateAndTime(DateTime? date, DateTime? time)
        {
            if (!date.HasValue) return null;

            return time.HasValue
                ? new DateTime(date.Value.Year,
                date.Value.Month,
                date.Value.Day,
                time.Value.Hour,
                time.Value.Minute,
                0)
                : new DateTime(date.Value.Year,
                date.Value.Month,
                date.Value.Day);
        }
    }
}
