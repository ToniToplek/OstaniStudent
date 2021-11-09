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
    public class PredmetiController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly PredmetiService _predmetiService;

        public PredmetiController(
            ServiceDb context,
            PredmetiService predmetiService
            )
        {
            _context = context;
            _predmetiService = predmetiService;
        }

        [HttpGet]
        [Route("getallpredmets")]
        public async Task<ActionResult> GetPredmets()
        {
            var result = await _predmetiService.GetAllPredmets();
            return Ok(result);
        }

        [HttpGet]
        [Route("getpredmetbyid")]
        public async Task<ActionResult> GetPredmetById(int id)
        {
            var result = await _predmetiService.GetPredmetById(id);
            return Ok(result);
        }


        [HttpPost]
        [Route("addpredmet")]
        public async Task<ActionResult<Predmeti>> AddPredmet(Predmeti predmet)
        {
            var result = await _predmetiService.AddPredmet (predmet);
            return Ok(result);
        }

        [HttpPut]
        [Route("updatepredmet")]
        public async Task<ActionResult<Predmeti>> UpdatePredmet(Predmeti predmet)
        {
            var result = await _predmetiService.UpdatePredmet(predmet);
            return Ok(result);
        }


        [HttpDelete]
        [Route("deletepredmet/{id}")]
        public async Task<ActionResult> DeletePredmet([FromRoute] int id)
        {
            var result = await _predmetiService.DeletePredmetById(id);
            return Ok(result);
        }

    }
}
