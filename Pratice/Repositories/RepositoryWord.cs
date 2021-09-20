using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Helpers;
using Pratice.Models;
using Pratice.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Repositories
{
    public class RepositoryWord : IRepositoryWord
    {
         private readonly MimicContext _context;

        public RepositoryWord(MimicContext context)
        {
            _context = context;
        }

        public PaginationList<Word> WordsGet(WordUrlQuery query)
        {

            var list = new PaginationList<Word>();
            var item = _context.Words.AsNoTracking().AsQueryable();
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
                    TotalPages = (int)Math.Ceiling((double)totalQuantRegister / query.QuantPage.Value)
                };

                list.Pagination = pagination;

            }
            list.AddRange(item.ToList());
            return list;
        }

        public Word Get(int id)
        {
            return _context.Words.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public void Register(Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
        }

        public void Update(int id, Word word)
        {
            _context.Words.Update(word);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var obj = Get(id);
            _context.Words.Remove(obj);
            _context.SaveChanges();
        }

    }
}
