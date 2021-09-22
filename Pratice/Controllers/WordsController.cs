using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pratice.Data;
using Pratice.Helpers;
using Pratice.Models;
using Pratice.Models.DTO;
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
        private readonly IMapper _mapper;

        public WordsController(IRepositoryWord repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //App -- /api/word?date=2021-05-10
        [HttpGet ("",Name = "GetAll")] // ação do metodo
        [Route("")]
        public IActionResult WordsGet([FromQuery] WordUrlQuery query)
        {
            var item = _repository.WordsGet(query);


            if (item.Results.Count == 0)
            {
                return NotFound();
            }

            PaginationList<WordDTO> list = CreateLinks(query, item);

            return Ok(list);
        }

        private PaginationList<WordDTO> CreateLinks(WordUrlQuery query, PaginationList<Word> item)
        {
            var list = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(item);

            foreach (var word in list.Results)
            {
                word.Links.Add(new LinkDTO("self", Url.Link("GetName", new { id = word.Id }), "GET"));
                // word.Links.Add(new LinkDTO("update", Url.Link("UpdateName", new { id = word.Id }), "PUT"));
                // word.Links.Add(new LinkDTO("delete", Url.Link("DeleteName", new { id = word.Id }), "DELETE"));
            }

            list.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (item.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination)); //convertendo para string usando Json

                if (query.Page + 1 <= item.Pagination.TotalPages)
                {
                    var queryString = new WordUrlQuery() { Page = query.Page + 1, QuantPage = query.QuantPage, Date = query.Date };
                    list.Links.Add(new LinkDTO("nextPage", Url.Link("GetAll", queryString), "GET"));
                }
                if (query.Page - 1 > 0)
                {
                    var queryString = new WordUrlQuery() { Page = query.Page - 1, QuantPage = query.QuantPage, Date = query.Date };
                    list.Links.Add(new LinkDTO("prevPage", Url.Link("GetAll", queryString), "GET"));
                }

            }

            return list;
        }

        //WEB -- /api/palavras/id
        [HttpGet ("{id}", Name = "GetName")]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var obj = _repository.Get(id);
            if(obj == null)
            {
                return NotFound(); //obj não encontrado
            }
            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(obj);
            wordDTO.Links = new List<LinkDTO>();
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetName", new { id = wordDTO.Id }), "Get"));
            wordDTO.Links.Add(new LinkDTO("update",Url.Link("UpdateName", new { id = wordDTO.Id }), "PUT"));
            wordDTO.Links.Add(new LinkDTO("delete",Url.Link("DeleteName", new { id = wordDTO.Id }), "DELETE"));

            return Ok(wordDTO);
        }

        //adcionando palavra
        [HttpPost]
        [Route("")]
        public IActionResult Register([FromBody] Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.Create = DateTime.Now;
            word.Active = true;
            _repository.Register(word);
            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetName", new { id = wordDTO.Id }), "Get"));

            return Created($"/api/word/{word.Id}", wordDTO);
        }

        [HttpPut ("{id}", Name = "UpdateName")]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] Word word)
        {
            var obj = _repository.Get(id);
            if (obj == null)
            {
                return NotFound(); //obj não encontrado
            }

            if (word == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.Id = id;
            word.Active = obj.Active;
            word.Create = obj.Create;
            word.Update = DateTime.Now;
            _repository.Update(id, word);
            

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("GetName", new { id = wordDTO.Id }), "Get"));

            return Ok();
        }

        [HttpDelete ("{id}", Name = "DeleteName")]
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

