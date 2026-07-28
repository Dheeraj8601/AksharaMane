using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Domain.Entities;
using AksharaMane.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AksharaManeDbContext _context;

        public BookRepository(AksharaManeDbContext context)
        {
            _context = context;
        }

        public IQueryable<Book> GetQueryable()
        {
            return _context.Books
                .Include(x => x.Category)
                .AsQueryable();
        }

        public async Task<Book?> GetByIdAsync(
            int id,
            bool trackChanges = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Books
                .Include(x => x.Category)
                .AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        }

        public async Task AddAsync(
            Book book,
            CancellationToken cancellationToken = default)
        {
            await _context.Books.AddAsync(book, cancellationToken);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
