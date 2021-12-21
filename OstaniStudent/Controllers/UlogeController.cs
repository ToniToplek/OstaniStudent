using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OstaniStudent.Database;
using OstaniStudent.Database.Models;
using OstaniStudent.Services;

namespace OstaniStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UlogeController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly UlogeService _ulogeService;
        private readonly KorisniciService _korisniciService;

        public UlogeController(
            ServiceDb context,
            UlogeService ulogeService,
            KorisniciService korisniciService
            )
        {
            _context = context;
            _ulogeService = ulogeService;
            _korisniciService = korisniciService;
        }

        [HttpGet]
        [Authorize]
        [Route("getallulogas")]
        public async Task<ActionResult> GetUlogas()
        {
            var result = await _ulogeService.GetAllUlogas();
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getulogabyuserid/{id}")]
        public async Task<ActionResult> GetUlogaById(int id)
        {
            var result = await _ulogeService.GetUlogaByClientId(id);
            return Ok(result);
        }


        [HttpGet]
        [Authorize]
        [Route("getulogabyuserbulkid/{bulkId}")]
        public async Task<ActionResult> GetUlogaByBulkId(string bulkId)
        {
            var user = await _korisniciService.GetUserByBulkId(bulkId);
            var result = await _ulogeService.GetUlogaByClientId(user.Id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("adduloga")]
        public async Task<ActionResult<Uloge>> AddUloga(Uloge uloga)
        {
            var result = await _ulogeService.AddUloga (uloga);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("updateuloga")]
        public async Task<ActionResult<Uloge>> UpdateUloga(Uloge uloga)
        {
            var result = await _ulogeService.UpdateUloga(uloga);
            return Ok(result);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin,Moderator")]
        [Route("deleteuloga/{id}")]
        public async Task<ActionResult> DeleteUloga([FromRoute] int id)
        {
            var result = await _ulogeService.DeleteUlogaById(id);
            return Ok(result);
        }

    }
}
