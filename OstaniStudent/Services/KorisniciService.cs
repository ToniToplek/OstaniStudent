using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

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
                foreach (var item in dbData)
                {
                    item.Lozinka = "";
                }
                return dbData;       
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Korisnici> GetUserByBulkId(string bulkId)
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.Where(t => t.BulkId == new Guid(bulkId)).AsNoTracking().FirstOrDefaultAsync();
                dbData.Lozinka = "";
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<Korisnici> GetUserById(string Id)
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.Where(t => t.Id.ToString() == Id).AsNoTracking().FirstOrDefaultAsync();
                dbData.Lozinka = "";
                return dbData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


        public async Task<Korisnici> GetUserByLoginData(Korisnici korisnik)
        {
            try
            {
                var dbData = await _dbContext.Korisnicis.Where(t => t.Email == korisnik.Email && t.JeAktivan).AsNoTracking().FirstOrDefaultAsync();

                if (dbData == null || !BC.Verify(korisnik.Lozinka, dbData.Lozinka))
                {
                    return null;
                }
                else
                {
                    return dbData;
                }

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
                user.Lozinka = BC.HashPassword(korisniciUloge.Lozinka);
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
                if(user.Lozinka.Length > 2) { 
                    oldUser.Lozinka = BC.HashPassword(user.Lozinka);
                }
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
                var korSelPred = await _dbContext.KorisniciPredmetis.Where(t => t.IdKorisnik == model[0].IdKorisnik && t.JeAktivan == true).ToListAsync();
                var korSelMod = await _dbContext.KorisnikZeljeniModuls.Where(t => t.IdKorisnik == model[0].IdKorisnik && t.JeAktivan == true).ToListAsync();

                if (korSelPred.Count > 0)
                {
                    foreach (var item in korSelPred)
                    {
                        item.JeAktivan = false;
                    }
                }

                if (korSelMod.Count > 0)
                {
                    foreach (var item in korSelMod)
                    {
                        item.JeAktivan = false;
                    }
                }

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

        public async Task<bool> IsUserAlreadyChoice(string bulkId)
        {
            try
            {
                var user = await this.GetUserByBulkId(bulkId);
                var dbData = await _dbContext.VKorisniciZeljeniModulis.Where(t => t.IdKorisnik == user.Id).AsNoTracking().FirstOrDefaultAsync();

                if (dbData != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        
        public async Task<KorisniciZeljeniModuliDto> GetUserModulChoice(int id)
        {
            try
            {
                var dbData = await _dbContext.VKorisniciZeljeniModulis.Where(t=>t.IdKorisnik == id).AsNoTracking().ToListAsync();

                var allData = new KorisniciZeljeniModuliDto();
                var ids = new List<int>();
                foreach (var item in dbData)
                {
                    if (ids.Contains(item.IdKorisnik))
                    {
                        var data = allData;
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
                        allData = data;
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

        public async Task<MemoryStream> ExportToExcel()
        {
            try
            {
                var allUsers = await GetAllUsersChoices();
                var allSubjects = await _dbContext.VKorisniciZeljeniPredmetis.Where(t => t.JeAktivan == true).AsNoTracking().ToListAsync();

                using var workbook = new XLWorkbook();

                foreach (var item in allUsers)
                {
                    var worksheet = workbook.Worksheets.Add(item.Ime+" "+item.Prezime);
                    var userSubjectsFirstWinter = allSubjects.Where(t => t.IdKorisnik == item.IdKorisnik && t.JeZimski == true && t.Rang == 1).OrderBy(t => t.BrojIzbora).ToList();
                    var userSubjectsSecondWinter = allSubjects.Where(t => t.IdKorisnik == item.IdKorisnik && t.JeZimski == true && t.Rang == 2).OrderBy(t => t.BrojIzbora).ToList();
                    var userSubjectsFirstSummer = allSubjects.Where(t => t.IdKorisnik == item.IdKorisnik && t.JeZimski == false && t.Rang == 1).OrderBy(t => t.BrojIzbora).ToList();
                    var userSubjectsSecondSummer = allSubjects.Where(t => t.IdKorisnik == item.IdKorisnik && t.JeZimski == false && t.Rang == 2).OrderBy(t => t.BrojIzbora).ToList();

                    worksheet.Cell(1, 1).Value = "Odabir studenta:";
                    worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 2)).Merge();
                    worksheet.Cell(1, 3).Value = item.Ime + " " + item.Prezime;
                    worksheet.Range(worksheet.Cell(1, 3), worksheet.Cell(1, 3)).Style.Font.Bold = true;


                    worksheet.Cell(3, 3).Value = "Prvi izbor";
                    worksheet.Cell(4, 3).Value = "Zimski semestar";
                    worksheet.Cell(5, 3).Value = "Izbor";
                    worksheet.Cell(5, 4).Value = "Predmet";
                    worksheet.Cell(5, 5).Value = "Modul";
                    worksheet.Range(worksheet.Cell(5, 3), worksheet.Cell(5, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(worksheet.Cell(3, 3), worksheet.Cell(5, 5)).Style.Font.Bold = true;
                    worksheet.Range(worksheet.Cell(3, 3), worksheet.Cell(4, 3)).Style.Font.FontSize = 14;

                    var row = 5;
                    foreach (var userSub in userSubjectsFirstWinter)
                    {
                        row++;
                        worksheet.Cell(row, 3).Value = userSub.BrojIzbora+".";
                        worksheet.Cell(row, 4).Value = userSub.Naziv;
                        if(userSub.Modul != null && userSub.Modul != "")
                        {
                            worksheet.Cell(row, 5).Value = userSub.Modul + " (" +userSub.Kratica+")";
                        }
                        else
                        {
                            worksheet.Cell(row, 5).Value = "Zajednički izborni predmet";
                        }
                    }
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    row = row+2;
                    worksheet.Cell(row, 3).Value = "Ljetni semestar";
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row+1, 5)).Style.Font.Bold = true;
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 3)).Style.Font.FontSize = 14;
                    row++;
                    worksheet.Cell(row, 3).Value = "Izbor";
                    worksheet.Cell(row, 4).Value = "Predmet";
                    worksheet.Cell(row, 5).Value = "Modul";
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    foreach (var userSub in userSubjectsFirstSummer)
                    {
                        row++;
                        worksheet.Cell(row, 3).Value = userSub.BrojIzbora + ".";
                        worksheet.Cell(row, 4).Value = userSub.Naziv;
                        if (userSub.Modul != null && userSub.Modul != "")
                        {
                            worksheet.Cell(row, 5).Value = userSub.Modul + " (" + userSub.Kratica + ")";
                        }
                        else
                        {
                            worksheet.Cell(row, 5).Value = "Zajednički izborni predmet";
                        }
                    }
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    row = row + 4;
                    worksheet.Cell(row, 3).Value = "Drugi izbor";
                    row++;
                    worksheet.Cell(row, 3).Value = "Zimski semestar";
                    worksheet.Range(worksheet.Cell(row - 1, 3), worksheet.Cell(row + 1, 5)).Style.Font.Bold = true;
                    worksheet.Range(worksheet.Cell(row - 1, 3), worksheet.Cell(row, 3)).Style.Font.FontSize = 14;
                    row++;
                    worksheet.Cell(row, 3).Value = "Izbor";
                    worksheet.Cell(row, 4).Value = "Predmet";
                    worksheet.Cell(row, 5).Value = "Modul";
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    foreach (var userSub in userSubjectsSecondWinter)
                    {
                        row++;
                        worksheet.Cell(row, 3).Value = userSub.BrojIzbora + ".";
                        worksheet.Cell(row, 4).Value = userSub.Naziv;
                        if (userSub.Modul != null && userSub.Modul != "")
                        {
                            worksheet.Cell(row, 5).Value = userSub.Modul + " (" + userSub.Kratica + ")";
                        }
                        else
                        {
                            worksheet.Cell(row, 5).Value = "Zajednički izborni predmet";
                        }
                    }

                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    row = row + 2;
                    worksheet.Cell(row, 3).Value = "Ljetni semestar";
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row + 1, 5)).Style.Font.Bold = true;
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 3)).Style.Font.FontSize = 14;
                    row++;
                    worksheet.Cell(row, 3).Value = "Izbor";
                    worksheet.Cell(row, 4).Value = "Predmet";
                    worksheet.Cell(row, 5).Value = "Modul";
                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    foreach (var userSub in userSubjectsSecondSummer)
                    {
                        row++;
                        worksheet.Cell(row, 3).Value = userSub.BrojIzbora + ".";
                        worksheet.Cell(row, 4).Value = userSub.Naziv;
                        if (userSub.Modul != null && userSub.Modul != "")
                        {
                            worksheet.Cell(row, 5).Value = userSub.Modul + " (" + userSub.Kratica + ")";
                        }
                        else
                        {
                            worksheet.Cell(row, 5).Value = "Zajednički izborni predmet";
                        }
                    }

                    worksheet.Range(worksheet.Cell(row, 3), worksheet.Cell(row, 5)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;



                    worksheet.Columns("C:E").AdjustToContents();
                }

                await using var memory = new MemoryStream();
                workbook.SaveAs(memory);

                return memory;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }


    }
}
