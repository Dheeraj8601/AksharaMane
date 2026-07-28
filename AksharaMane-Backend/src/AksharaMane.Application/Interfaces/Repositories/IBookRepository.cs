using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetQueryable();

        Task<Book?> GetByIdAsync(int id,bool trackChanges = false,CancellationToken cancellationToken = default);

        Task AddAsync(Book book,CancellationToken cancellationToken = default);

        void Update(Book book);

        void Delete(Book book);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
