using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;
using OstaniStudent.Services;

namespace OstaniStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly KorisniciService _korisniciService;
        private readonly UlogeService _ulogeService;
        public KorisniciController(
            ServiceDb context,
            KorisniciService korisniciService,
            UlogeService ulogeService
            )
        {
            _context = context;
            _korisniciService = korisniciService;
            _ulogeService = ulogeService;
        }

        [HttpGet]
        [Authorize]
        [Route("getallusers")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _korisniciService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getalluserschoice")]
        public async Task<ActionResult> GetUsersChoice()
        {
            var result = await _korisniciService.GetAllUsersChoices();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getusersmodulchoice/{id}")]
        public async Task<ActionResult> GetUserModulChoice([FromRoute] int id)
        {
            var result = await _korisniciService.GetUserModulChoice(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getuserchoice/{bulkId}")]
        public async Task<ActionResult> IsUserAlreadyChoice([FromRoute] string bulkId)
        {
            var result = await _korisniciService.IsUserAlreadyChoice(bulkId);
            return Ok(result);
        }


        [HttpGet]
        [Authorize]
        [Route("getalluserssubjectchoice/{odabir}/{korisnikId}")]
        public async Task<ActionResult> GetUsersSubjectChoice([FromRoute] int odabir, [FromRoute] int korisnikId)
        {
            var result = await _korisniciService.GetAllUsersSubjectChoices(korisnikId, odabir);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getuserbybulkid/{bulkId}")]
        public async Task<ActionResult> GetUserById([FromRoute] string bulkId)
        {
            var result = await _korisniciService.GetUserByBulkId(bulkId);
            return Ok(result);
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(Korisnici korisnik)
        {
            var result = await _korisniciService.GetUserByLoginData(korisnik);

            if(result != null)
            {
                var role = await _ulogeService.GetUlogaByClientId(result.Id);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", result.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role.Naziv)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PrfVmfuuO6qxsutb2a5HYPutoPOcoiyU")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Email ili lozinka neispravni." });
            }

        }

        [HttpGet]
        [Authorize]
        [Route("getuserbylogindata")]
        public async Task<ActionResult> LoginData()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var result = await _korisniciService.GetUserById(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Email ili lozinka neispravni." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("adduser")]
        public async Task<ActionResult<Korisnici>> AddUser(VKorisniciUloge korisnik)
        {
            var result = await _korisniciService.AddUser(korisnik);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("updateuser")]
        public async Task<ActionResult<Korisnici>> UpdateUser(VKorisniciUloge korisnik)
        {
            var result = await _korisniciService.UpdateUser(korisnik);
            return Ok(result);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("deleteuserbyid/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _korisniciService.DeleteUserById(id);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("deleteuserchoicebyid/{id}")]
        public async Task<ActionResult> DeleteUserChoice([FromRoute] int id)
        {
            var result = await _korisniciService.DeleteUserChoiceById(id);
            return Ok(result);
        }


        [HttpPost]
        [Authorize]
        [Route("addchoice")]
        public async Task<ActionResult<Korisnici>> SaveStudentChoice(OstaniStudentDto[] model)
        {
            var result = await _korisniciService.SaveStudentChoice(model);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var stream = await _korisniciService.ExportToExcel();

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


    }
}
