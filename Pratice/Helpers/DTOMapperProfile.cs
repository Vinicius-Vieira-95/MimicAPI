using AutoMapper;
using Pratice.Models;
using Pratice.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Helpers
{
    public class DTOMapperProfile : Profile
    {
      
        public DTOMapperProfile()
        {
            CreateMap<Word, WordDTO>();
            CreateMap<PaginationList<Word>, PaginationList<WordDTO>>();
        }
    }
}
