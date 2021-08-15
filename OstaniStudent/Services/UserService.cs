using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aestus.eKale2.Microservice.FinancialAccounting.Services
{

    public class UserService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<UserService> _logger;
        public UserService(
            ServiceDb dbContext, 
            ILogger<UserService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Korisnici>> GetAllUsers()
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.AsNoTracking().ToListAsync();
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

    }
}
