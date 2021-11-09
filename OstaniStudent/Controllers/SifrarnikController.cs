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
    public class SifrarnikController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly SifrarnikService _sifrarnikService;

        public SifrarnikController(
            ServiceDb context,
            SifrarnikService sifrarnikService
            )
        {
            _context = context;
            _sifrarnikService = sifrarnikService;
        }

        [HttpGet]
        [Route("getallsifrarniks")]
        public async Task<ActionResult> GetSifrarniks()
        {
            var result = await _sifrarnikService.GetAllSifrarniks();
            return Ok(result);
        }

        [HttpGet]
        [Route("getsifrarnikbyid")]
        public async Task<ActionResult> GetSifrarnikById(int id)
        {
            var result = await _sifrarnikService.GetSifrarnikById(id);
            return Ok(result);
        }


        [HttpPost]
        [Route("addsifrarnik")]
        public async Task<ActionResult<Sifrarnik>> AddSifrarnik(Sifrarnik sifrarnik)
        {
            var result = await _sifrarnikService.AddSifrarnik (sifrarnik);
            return Ok(result);
        }

        [HttpPut]
        [Route("updatesifrarnik")]
        public async Task<ActionResult<Sifrarnik>> UpdateSifrarnik(Sifrarnik sifrarnik)
        {
            var result = await _sifrarnikService.UpdateSifrarnik(sifrarnik);
            return Ok(result);
        }


        [HttpDelete]
        [Route("deletesifrarnik/{id}")]
        public async Task<ActionResult> DeleteSifrarnik([FromRoute] int id)
        {
            var result = await _sifrarnikService.DeleteSifrarnikById(id);
            return Ok(result);
        }

    }
}
