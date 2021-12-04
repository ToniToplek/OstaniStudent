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

    public class ModuliService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<ModuliService> _logger;
        public ModuliService(
            ServiceDb dbContext, 
            ILogger<ModuliService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Moduli>> GetAllModuls()
        {
            try
            {
                var dbData = await _dbContext.Modulis.AsNoTracking().Where(t => t.JeAktivan).ToListAsync();
                dbData.OrderBy(t => t.Naziv);
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Moduli> GetModulById(int id)
        {
            try
            {
                var dbData = await _dbContext.Modulis.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Moduli> AddModul(Moduli modul)
        {
            try
            {
                modul.JeAktivan = true;
                await _dbContext.Modulis.AddAsync(modul);
                await _dbContext.SaveChangesAsync();

                return modul;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Moduli> UpdateModul(Moduli modul)
        {
            try
            {
                var dbData = _dbContext.Modulis.Where(t => t.Id == modul.Id).FirstOrDefault();
                dbData.Naziv = modul.Naziv;
                dbData.Kratica = modul.Kratica;
                _dbContext.Update(dbData);
                await _dbContext.SaveChangesAsync();

                return modul;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<bool> DeleteModulById(int id)
        {
            try
            {

                var dbData = _dbContext.Modulis.Where(t => t.Id == id).FirstOrDefault();
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
