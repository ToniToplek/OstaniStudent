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

    public class KorisniciService
    {
        private readonly ServiceDb _dbContext;
        private readonly ILogger<KorisniciService> _logger;
        public KorisniciService(
            ServiceDb dbContext, 
            ILogger<KorisniciService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<List<Korisnici>> GetAllUsers()
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.Where(t => !!t.JeAktivan).AsNoTracking().ToListAsync();
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Korisnici> GetUserById(int id)
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.Where(t => t.Id == id).AsNoTracking().FirstOrDefaultAsync();
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Korisnici> AddUser(Korisnici user)
        {
            try
            {
                user.BulkId = Guid.NewGuid();
                user.JeAktivan = true;
                await _dbContext.Korisnicis.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Korisnici> UpdateUser(Korisnici user)
        {
            try
            {
                var dbData = _dbContext.Korisnicis.Where(t => t.Id == user.Id).FirstOrDefault();
                dbData.Ime = user.Ime;
                dbData.Prezime = user.Prezime;
                dbData.Jmbag = user.Jmbag;
                dbData.Email = user.Email;
                _dbContext.Update(dbData);
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<bool> DeleteUserById(int id)
        {
            try
            {

                var dbData = _dbContext.Korisnicis.Where(t => t.Id == id).FirstOrDefault();
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
