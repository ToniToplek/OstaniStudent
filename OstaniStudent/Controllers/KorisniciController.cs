﻿using System;
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
    public class KorisniciController : ControllerBase
    {
        private readonly ServiceDb _context;
        private readonly KorisniciService _korisniciService;

        public KorisniciController(
            ServiceDb context,
            KorisniciService korisniciService 
            )
        {
            _context = context;
            _korisniciService = korisniciService;
        }

        [HttpGet]
        [Route("getallusers")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _korisniciService.GetAllUsers();
            return Ok(result);
        }

        [HttpGet]
        [Route("getuserbyid/{id}")]
        public async Task<ActionResult> GetUserById([FromRoute] int id)
        {
            var result = await _korisniciService.GetUserById(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("adduser")]
        public async Task<ActionResult<Korisnici>> AddUser(Korisnici korisnik)
        {
            var result = await _korisniciService.AddUser(korisnik);
            return Ok(result);
        }

        [HttpPut]
        [Route("updateuser")]
        public async Task<ActionResult<Korisnici>> UpdateUser(Korisnici korisnik)
        {
            var result = await _korisniciService.UpdateUser(korisnik);
            return Ok(result);
        }


        [HttpDelete]
        [Route("deleteuserbyid/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _korisniciService.DeleteUserById(id);
            return Ok(result);
        }

    }
}
