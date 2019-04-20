﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Services
{
    public interface IAssertX86ServerService
    {
        Task<List<Assert_X86Server>> GetX86ServersAsync(string keyword, int pageIndex, int pageSize);

        Task<int> CountAsync(string keyword);

        Task<Assert_X86Server> GetAssertX86ServerById(Guid id);

        Task<Assert_X86Server> AddAssertX86Server(Assert_X86Server request);

        Task<Assert_X86Server> UpdateAssertX86Server(Assert_X86Server entity);

        Task<Assert_X86Server> GetAssertX86ServerByColumn(String value, Util.ENUMS.Type type);

        Task<bool> RemoveAssertX86ServerByGuid(Guid id);
    }

    public class AssertX86ServerService : IAssertX86ServerService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public AssertX86ServerService(DataCenterOperationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<AssertX86ServerService>();
        }

        public async Task<List<Assert_X86Server>> GetX86ServersAsync(string keyword, int pageIndex, int pageSize)
        {
            var query = PrepareQuery(keyword);

            return await query.OrderByDescending(f => f.CreatedTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string keyword)
        {
            return await PrepareQuery(keyword).CountAsync();
        }

        private IQueryable<Assert_X86Server> PrepareQuery(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? from s in _db.Assert_X86Servers select s
                : from s in _db.Assert_X86Servers
                  where s.CPU.Contains(keyword)
                  || s.Memory.Contains(keyword)
                  || s.HD.Contains(keyword)
                  || s.OS.Contains(keyword)
                  || s.RackLocation.Contains(keyword)
                  || s.SerialNumber.Contains(keyword)
                  || s.ContractNumber.Contains(keyword)
                  select s;
        }

        public async Task<Assert_X86Server> AddAssertX86Server(Assert_X86Server request)
        {
            request.Id = Guid.NewGuid();

            _db.Assert_X86Servers.Add(request);

            await _db.SaveChangesAsync();

            return request;
        }

        public async Task<Assert_X86Server> UpdateAssertX86Server(Assert_X86Server entity)
        {
            var currentEntity = _db.Assert_X86Servers.FirstOrDefault(f => f.Id == entity.Id);

            currentEntity = entity;

            await _db.SaveChangesAsync();
            return currentEntity;
        }

        public async Task<Assert_X86Server> GetAssertX86ServerById(Guid id)
        {
            return await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Assert_X86Server> GetAssertX86ServerByColumn(String value, Util.ENUMS.Type type)
        {
            Assert_X86Server returnServer = null;
            switch (type)
            {

                case ENUMS.Type.FixedAssertNumber:
                    returnServer = await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.FixedAssertNumber == value);
                    break;

                default:
                    returnServer = null;
                    break;
            }
            return returnServer;
        }

        public async Task<bool> RemoveAssertX86ServerByGuid(Guid id)
        {
            var x86Server = await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.Id == id);
            if (x86Server == null)
            {
                return false;
            }

            try
            {
                _db.Assert_X86Servers.Remove(x86Server);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return false;
            }
        }

    }
}