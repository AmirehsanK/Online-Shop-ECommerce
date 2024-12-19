using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public class Paging
    {
        public class BasePaging<T> where T : class
        {
            public List<T> Entities { get; set; }
            public int Skip { get; set; }
            public int Page { get; set; } = 1;
            public int PageCount { get; set; }

            public int TakeEntity { get; set; } = 10;

            public async Task<BasePaging<T>> Paging(IQueryable<T> source) 
            {
                int totalItems =await source.CountAsync();
                Skip = (Page - 1) * TakeEntity;
                PageCount = (int)Math.Ceiling(totalItems / (double)TakeEntity);
                Entities = await source.Skip(Skip).Take(TakeEntity).ToListAsync();

                return this;
            }
        }
        public class PagingViewModel
        {
            public int StartPage { get; set; } = 1;
            public int Page { get; set; } 

            public int EndPage { get; set; }
        }

   
    }
}