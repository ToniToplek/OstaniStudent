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

    public class SifrarnikService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<SifrarnikService> _logger;
        public SifrarnikService(
            ServiceDb dbContext, 
            ILogger<SifrarnikService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Sifrarnik>> GetAllSifrarniks()
        {
            try
            {
                var dbData = await _dbContext.Sifrarniks.AsNoTracking().ToListAsync();
                dbData.OrderBy(t => t.Naziv);
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Sifrarnik> GetSifrarnikById(int id)
        {
            try
            {
                var dbData = await _dbContext.Sifrarniks.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Sifrarnik> AddSifrarnik(Sifrarnik sifrarnik)
        {
            try
            {
                await _dbContext.Sifrarniks.AddAsync(sifrarnik);
                await _dbContext.SaveChangesAsync();

                return sifrarnik;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Sifrarnik> UpdateSifrarnik(Sifrarnik sifrarnik)
        {
            try
            {
                var dbData = _dbContext.Sifrarniks.Where(t => t.Id == sifrarnik.Id).FirstOrDefault();
                dbData.Naziv = sifrarnik.Naziv;
                _dbContext.Update(dbData);
                await _dbContext.SaveChangesAsync();

                return sifrarnik;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<bool> DeleteSifrarnikById(int id)
        {
            try
            {

                var dbData = _dbContext.Sifrarniks.Where(t => t.Id == id).FirstOrDefault();
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
