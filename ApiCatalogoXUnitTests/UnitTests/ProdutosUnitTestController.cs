using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi___Sec3.Context;
using WebApi___Sec3.DTOs.Mappings;
using WebApi___Sec3.Repositories;

namespace ApiCatalogoXUnitTests.UnitTests
{
    public class ProdutosUnitTestController
    {


        public IUnitOfWork repository;
        public IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connString = "Server=DESKTOP-7DFKLS9\\SQLHOME;Database=ClienteDB;User Id=sa;Password=C47541240;TrustServerCertificate=True";
            
        static ProdutosUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connString)
                .Options;
        }

        public ProdutosUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProdutoDTOMappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);
            repository = new UnitOfWork(context);
        }

    
            
            
    }
}
