using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkysController : ControllerBase
    {
        private ISkyRepository repository;
        public SkysController(ISkyRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{city}/{days:int=15}")]
        public async Task<IQueryable<Sky>> GetSkys(string city, int days)
        {
            return await repository.GetSkys(city, days);
        }
        
    }
}
