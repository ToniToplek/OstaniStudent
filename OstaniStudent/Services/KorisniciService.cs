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

        public async Task<List<VKorisniciUloge>> GetAllUsers()
        {
            try
            {
                var dbData = await _dbContext.VKorisniciUloges.AsNoTracking().ToListAsync();
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

        public async Task<int> AddUser(VKorisniciUloge korisniciUloge)
        {
            try
            {
                var user = new Korisnici();
                user.BulkId = Guid.NewGuid();
                user.Ime = korisniciUloge.Ime;
                user.Prezime = korisniciUloge.Prezime;
                user.Jmbag = korisniciUloge.Jmbag;
                user.Email = korisniciUloge.Email;
                user.JeAktivan = true;
                await _dbContext.Korisnicis.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var korUl = new KorisniciUloge();
                korUl.IdKorisnik = user.Id;
                korUl.IdUloge = (int)korisniciUloge.UlogaId;
                korUl.JeAktivan = true;

                await _dbContext.KorisniciUloges.AddAsync(korUl);
                await _dbContext.SaveChangesAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<VKorisniciUloge> UpdateUser(VKorisniciUloge user)
        {
            try
            {
                var oldUser = _dbContext.Korisnicis.Where(t => t.Id == user.KorisnikId).FirstOrDefault();
                oldUser.Ime = user.Ime;
                oldUser.Prezime = user.Prezime;
                oldUser.Jmbag = user.Jmbag;
                oldUser.Email = user.Email;
                 _dbContext.Korisnicis.Update(oldUser);

                var oldKorUl = _dbContext.KorisniciUloges.Where(t => t.Id == user.KorisniciUlogeId).FirstOrDefault();
                oldKorUl.IdUloge = (int)user.UlogaId;
                 _dbContext.KorisniciUloges.Update(oldKorUl);

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

                var korUl = _dbContext.KorisniciUloges.Where(t => t.IdKorisnik == id).ToList();
                foreach (var item in korUl)
                {
                    item.JeAktivan = false;
                }

                await _dbContext.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteUserChoiceById(int id)
        {
            try
            {

                var dbData = _dbContext.KorisnikZeljeniModuls.Where(t => t.IdKorisnik == id).ToList();

                foreach (var item in dbData)
                {
                    item.JeAktivan = false;
                }

                var predmets = _dbContext.KorisniciPredmetis.Where(t => t.IdKorisnik == id).ToList();

                foreach (var item in predmets)
                {
                    item.JeAktivan = false;
                }

                await _dbContext.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> SaveStudentChoice(OstaniStudentDto[] model)
        {
            try
            {
                var rangList = new List<int>();
                foreach (var item in model)
                {
                    var korPred = new KorisniciPredmeti();
                    korPred.IdKorisnik = item.IdKorisnik;
                    korPred.IdPredmet = item.IdPredmet;
                    korPred.Rang = item.Rang;
                    korPred.BrojIzbora = item.BrojIzbora;
                    korPred.JeAktivan = true;
                    await _dbContext.KorisniciPredmetis.AddAsync(korPred);

                    if (!rangList.Contains(item.Rang)) {
                        rangList.Add(item.Rang);
                        var korMod = new KorisnikZeljeniModul();
                        korMod.IdKorisnik = item.IdKorisnik;
                        korMod.IdModul = item.IdModul;
                        korMod.Rang = item.Rang;
                        korMod.JeAktivan = true;
                        await _dbContext.KorisnikZeljeniModuls.AddAsync(korMod);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }



        public async Task<List<KorisniciZeljeniModuliDto>> GetAllUsersChoices()
        {
            try
            {
                var dbData = await _dbContext.VKorisniciZeljeniModulis.AsNoTracking().ToListAsync();

                var allData = new List<KorisniciZeljeniModuliDto>();
                var ids = new List<int>();
                foreach (var item in dbData)
                {
                    if (ids.Contains(item.IdKorisnik))
                    {
                        var data = allData.FirstOrDefault(t => t.IdKorisnik == item.IdKorisnik);
                        if (item.Rang == 1)
                        {
                            data.PrviIzbor = item.Naziv;
                            data.PrviIzborModulId = item.IdModul;
                        }
                        else if (item.Rang == 2)
                        {
                            data.DrugiIzbor = item.Naziv;
                            data.DrugiIzborModulId = item.IdModul;
                        }
                    }
                    else
                    {
                        var data = new KorisniciZeljeniModuliDto();
                        data.IdKorisnik = item.IdKorisnik;
                        data.Ime = item.Ime;
                        data.Prezime = item.Prezime;
                        if(item.Rang == 1)
                        {
                            data.PrviIzbor = item.Naziv;
                            data.PrviIzborModulId = item.IdModul;
                        }
                        else if(item.Rang == 2)
                        {
                            data.DrugiIzbor = item.Naziv;
                            data.DrugiIzborModulId = item.IdModul;
                        }
                        allData.Add(data);
                        ids.Add(data.IdKorisnik);
                    }
                }

                return allData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<List<VKorisniciZeljeniPredmeti>> GetAllUsersSubjectChoices(int korisnikId, int odabir)
        {
            try
            {
                var dbData = await _dbContext.VKorisniciZeljeniPredmetis.Where(t => t.IdKorisnik == korisnikId && t.Rang == odabir).AsNoTracking().ToListAsync();
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
