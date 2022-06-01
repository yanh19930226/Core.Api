using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Models.Mongos;

namespace Tools.Api.Services
{
    public interface IBookService
    {
        IList<Book> Get();

        Task<IList<Book>> GetAsync();

        Book Get(string id);

        Task<Book> GetAsync(string id);

        Book Create(Book book);

        Task<Book> CreateAsync(Book book);

        void Update(string id, Book bookIn);

        Task UpdateAsync(string id, Book bookIn);

        void Remove(string id);

        Task RemoveAsync(string id);
    }
}
