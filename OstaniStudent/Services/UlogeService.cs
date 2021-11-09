using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OstaniStudent.Services
{

    public class UlogeService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<UlogeService> _logger;
        public UlogeService(
            ServiceDb dbContext, 
            ILogger<UlogeService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Uloge>> GetAllUlogas()
        {
            try
            {
                var dbData = await _dbContext.Uloges.AsNoTracking().ToListAsync();
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Uloge> GetUlogaById(int id)
        {
            try
            {
                var dbData = await _dbContext.Uloges.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Uloge> AddUloga(Uloge uloga)
        {
            try
            {
                await _dbContext.Uloges.AddAsync(uloga);
                await _dbContext.SaveChangesAsync();

                return uloga;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Uloge> UpdateUloga(Uloge uloga)
        {
            try
            {
                var dbData = _dbContext.Uloges.Where(t => t.Id == uloga.Id).FirstOrDefault();
                dbData.Naziv = uloga.Naziv;
                _dbContext.Update(dbData);
                await _dbContext.SaveChangesAsync();

                return uloga;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<bool> DeleteUlogaById(int id)
        {
            try
            {

                var dbData = _dbContext.Uloges.Where(t => t.Id == id).FirstOrDefault();
                dbData.JeAktivan = false;

                await _dbContext.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

    }
}
