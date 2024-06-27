using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    public class CompletedOrderReadRepository:ReadRepository<CompletedOrder>,ICompletedOrderReadRepository
    {
        public CompletedOrderReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
