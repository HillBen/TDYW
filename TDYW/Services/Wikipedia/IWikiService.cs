using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Services
{
    interface IWikiService
    {
        List<pageval> FindPeople(string term, int limit = 10, int offset = 0, bool living = true);
        pageval GetPerson(int pageId);
    }
}
