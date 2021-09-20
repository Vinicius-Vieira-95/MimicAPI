using Pratice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pratice.Helpers;

namespace Pratice.Repositories.Contracts
{
    public interface IRepositoryWord
    {
        PaginationList<Word> WordsGet(WordUrlQuery query);

        Word Get(int id);

        void Register(Word word);

        void Update(int id, Word word);

        void Delete(int id);
    }
}
