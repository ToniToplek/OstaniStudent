using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public UlogeController(
            ServiceDb context,
            UlogeService ulogeService
            )
        {
            _context = context;
            _ulogeService = ulogeService;
        }

        [HttpGet]
        [Route("getallulogas")]
        public async Task<ActionResult> GetUlogas()
        {
            var result = await _ulogeService.GetAllUlogas();
            return Ok(result);
        }

        [HttpGet]
        [Route("getulogabyid")]
        public async Task<ActionResult> GetUlogaById(int id)
        {
            var result = await _ulogeService.GetUlogaById(id);
            return Ok(result);
        }


        [HttpPost]
        [Route("adduloga")]
        public async Task<ActionResult<Uloge>> AddUloga(Uloge uloga)
        {
            var result = await _ulogeService.AddUloga (uloga);
            return Ok(result);
        }

        [HttpPut]
        [Route("updateuloga")]
        public async Task<ActionResult<Uloge>> UpdateUloga(Uloge uloga)
        {
            var result = await _ulogeService.UpdateUloga(uloga);
            return Ok(result);
        }


        [HttpDelete]
        [Route("deleteuloga/{id}")]
        public async Task<ActionResult> DeleteUloga([FromRoute] int id)
        {
            var result = await _ulogeService.DeleteUlogaById(id);
            return Ok(result);
        }

    }
}
