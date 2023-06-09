﻿using AutoMapper;
using Companies.API.Data;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Companies.API.Filters;
using Companies.API.Repositories;
using Companies.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Companies.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public CompaniesController(IServiceManager serviceManager) => this.serviceManager = serviceManager;
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(bool includeEmployees = false) => 
            Ok(await serviceManager.CompanyService.GetCompaniesAsync(includeEmployees));
        

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid id) => 
            Ok(await serviceManager.CompanyService.GetAsync(id));


        [HttpPut("{id}")]
        [MatchingKeysFilter]
        public async Task<IActionResult> PutCompany(Guid id, CompanyForUpdateDto dto)
        {
            await serviceManager.CompanyService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(CompanyForCreationDto company)
        {
            var createdCompany = await serviceManager.CompanyService.CreateCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await serviceManager.CompanyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
