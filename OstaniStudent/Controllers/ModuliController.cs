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
    public class ModuliController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly ModuliService _moduliService;

        public ModuliController(
            ServiceDb context,
            ModuliService moduliService
            )
        {
            _context = context;
            _moduliService = moduliService;
        }

        [HttpGet]
        [Route("getallmoduls")]
        public async Task<ActionResult> GetModuls()
        {
            var result = await _moduliService.GetAllModuls();
            return Ok(result);
        }

        [HttpGet]
        [Route("getmodulbyid")]
        public async Task<ActionResult> GetModulById(int id)
        {
            var result = await _moduliService.GetModulById(id);
            return Ok(result);
        }


        [HttpPost]
        [Route("addmodul")]
        public async Task<ActionResult<Moduli>> AddModul(Moduli modul)
        {
            var result = await _moduliService.AddModul(modul);
            return Ok(result);
        }

        [HttpPut]
        [Route("updatemodul")]
        public async Task<ActionResult<Moduli>> UpdateModul(Moduli modul)
        {
            var result = await _moduliService.UpdateModul(modul);
            return Ok(result);
        }


        [HttpDelete]
        [Route("deletemodul/{id}")]
        public async Task<ActionResult> DeleteModul([FromRoute] int id)
        {
            var result = await _moduliService.DeleteModulById(id);
            return Ok(result);
        }

    }
}
