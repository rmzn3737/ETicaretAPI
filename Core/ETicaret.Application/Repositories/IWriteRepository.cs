using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Repositories
{
    public interface IWriteRepository<T>:IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);//Birden fazla veri geldi mesela koleksiyon bununla ekliyoruz.
        bool Remove(T model);
        Task<bool> RemoveAsync(string id); //Bunlarda işlemi yapınca sonucu true ya da false döndürmek için bool verdik.
        bool RemoveRange(List<T> datas);
        bool Update(T model);
        Task<int> SaveAsync();
    }
}
