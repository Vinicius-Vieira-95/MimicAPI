using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pratice.Data;
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
        public IActionResult WordsGet(DateTime? date)
        {
            var item = _context.Words.AsQueryable();
            if (date.HasValue)
            {
                item = item.Where(x => x.Create > date.Value || x.Update > date.Value);
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
