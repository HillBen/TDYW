using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage
    {
        get
        {
            return (PageIndex > 1);
        }
    }

    public bool HasNextPage
    {
        get
        {
            return (PageIndex < TotalPages);
        }
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        
        if (pageSize < 1) {
            pageSize = 10;                                  //if pageSize is less than minimum, set it to mimimum
        }
        else if(pageSize > 100)
        {
            pageSize = 100;                                 //if pageSize is more than maximum, set it to maximum
        }
        
        if (pageIndex < 1)
        {
            pageIndex = 1;                                  //if pageIndex is less than one, set it to 1
        }
        else if (count <= (pageIndex - 1) * pageSize)
        {
            pageIndex = (count + pageSize - 1) / pageSize;  //if pageIndex is greater than the total page count, set it to the last page
        }
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
