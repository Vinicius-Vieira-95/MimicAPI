using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pratice.Data;
using Pratice.Helpers;
using Pratice.Models;
using Pratice.Repositories;
using Pratice.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Controllers
{
    [Route("api/words")]
    public class WordsController : Controller
    {
        
        private readonly IRepositoryWord _repository;

        public WordsController(IRepositoryWord repository)
        {
            _repository = repository;     
        }

        //App -- /api/word?date=2021-05-10
        [HttpGet] // ação do metodo
        [Route("")]
        public IActionResult WordsGet([FromQuery] WordUrlQuery query)
        {
            var item = _repository.WordsGet(query);

            if (query.Page > item.Pagination.TotalPages)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination)); //convertendo para string usando Json
            return Ok(item.ToList());
        }
        
        //WEB -- /api/palavras/id
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var obj = _repository.Get(id);
            if(obj == null)
            {
                return NotFound(); //obj não encontrado
            }
            return Ok(obj);
        }

        //adcionando palavra
        [HttpPost]
        [Route("")]
        public IActionResult Register([FromBody] Word word)
        {

            _repository.Register(word);
            return Created($"/api/word/{word.Id}", word);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] Word word)
        {
            var obj = _repository.Get(id);
            if (obj == null)
            {
                return NotFound(); //obj não encontrado
            }

            //word.Id = id;
            _repository.Update(id, word);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _repository.Get(id);
            if (obj == null)
            {
                return NotFound(); //obj não encontrado
            }
            _repository.Delete(id);
           
            return NoContent(); // comment
        }
    }
}

