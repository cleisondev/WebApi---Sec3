﻿using WebApi___Sec3.Context;

namespace WebApi___Sec3.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IProdutoRepository _produtoRepo;

        public ICategoriaRepository _categoriaRepo;


        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }


        public async Task CommitAsync()
        {
         await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
