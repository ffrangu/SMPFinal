using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Models
{
    public interface IGenericRepository<T> where T: class
    {
        public Task<T> Get(object Id);

        public Task<IEnumerable<T>> GetAll();

        public Task<T> AddAsync(T obj);

        public Task<T> Update(T obj);

        public Task<T> Delete(object Id);


    }
}
