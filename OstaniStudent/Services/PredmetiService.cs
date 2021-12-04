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

    public class PredmetiService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<PredmetiService> _logger;
        public PredmetiService(
            ServiceDb dbContext, 
            ILogger<PredmetiService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Predmeti>> GetAllPredmets()
        {
            try
            {
                var dbData = await _dbContext.Predmetis.Where(t => t.JeAktivan).AsNoTracking().ToListAsync();
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<List<Predmeti>> GetRequiredPredmets(bool isRequired, int selectedModulId)
        {
            try
            {
                if (isRequired) { 
                    var dbData = await _dbContext.Predmetis.Where(t => (t.IdSifrarnik == 1 || t.IdModul == selectedModulId) && t.JeAktivan).AsNoTracking().ToListAsync();
                    return dbData;
                }
                else
                {
                    var dbData = await _dbContext.Predmetis.Where(t => t.IdSifrarnik != 1 && t.IdModul != selectedModulId && t.JeAktivan).AsNoTracking().ToListAsync();
                    return dbData;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Predmeti> GetPredmetById(int id)
        {
            try
            {
                var dbData = await _dbContext.Predmetis.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Predmeti> AddPredmet(Predmeti predmet)
        {
            try
            {
                predmet.JeAktivan = true;
                await _dbContext.Predmetis.AddAsync(predmet);
                await _dbContext.SaveChangesAsync();

                return predmet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Predmeti> UpdatePredmet(Predmeti predmet)
        {
            try
            {
                var dbData = _dbContext.Predmetis.Where(t => t.Id == predmet.Id).FirstOrDefault();
                dbData.Naziv = predmet.Naziv;
                dbData.Kapacitet = predmet.Kapacitet;
                dbData.IdModul = predmet.IdModul;
                dbData.IdSifrarnik = predmet.IdSifrarnik;
                dbData.JeZimski = predmet.JeZimski;
                _dbContext.Update(dbData);
                await _dbContext.SaveChangesAsync();

                return predmet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<bool> DeletePredmetById(int id)
        {
            try
            {

                var dbData = _dbContext.Predmetis.Where(t => t.Id == id).FirstOrDefault();
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
