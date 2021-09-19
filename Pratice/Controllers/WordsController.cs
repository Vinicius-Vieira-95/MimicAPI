using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pratice.Data;
using Pratice.Helpers;
using Pratice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Controllers
{
    [Route("api/words")]
    public class WordsController : Controller
    {
        
        private readonly MimicContext _context;

        public WordsController(MimicContext context)
        {
            _context = context;     
        }

        //App -- /api/word?date=2021-05-10
        [HttpGet] // ação do metodo
        [Route("")]
        public IActionResult WordsGet([FromQuery] WordUrlQuery query)
        {
            var item = _context.Words.AsQueryable();
            if (query.Date.HasValue)
            {
                item = item.Where(x => x.Create > query.Date.Value || x.Update > query.Date.Value);
            }
            if (query.Page.HasValue)
            {
                var totalQuantRegister = item.Count();
                item = item.Skip((query.Page.Value - 1) * query.QuantPage.Value).Take(query.QuantPage.Value);

                var pagination = new Pagination
                {
                    Page = query.Page.Value,
                    QuantPages = query.QuantPage.Value,
                    TotalRegisters = totalQuantRegister,
                    totalPages = (int)Math.Ceiling((double)totalQuantRegister / query.QuantPage.Value)
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination)); //convertendo para string usando Json

                if(query.Page > pagination.totalPages)
                {
                    return NotFound();
                }
            }
            return Ok(item);
        }
        
        //WEB -- /api/palavras/id
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var obj = _context.Words.Find(id);
            if(obj == null)
            {
                return NotFound(); //obj não encontrado
            }
            return Ok(_context.Words.Find(id));
        }

        //adcionando palavra
        [HttpPost]
        [Route("")]
        public IActionResult Register([FromBody] Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
            return Created($"/api/word/{word.Id}", word);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] Word word)
        {
            var obj = _context.Words.AsNoTracking().Where(x => x.Id == id); //ainda não sei
            if (obj == null)
            {
                return NotFound(); //obj não encontrado
            }

            word.Id = id;
            _context.Words.Update(word);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _context.Words.Find(id);
            if (obj == null)
            {
                return NotFound(); //obj não encontrado
            }

            _context.Words.Remove(_context.Words.Find(id));
            _context.SaveChanges();
            return NoContent(); // comment
        }
    }
}

