
using Api.Core.Entities.Failure;
using Api.Infra.ModelsConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Infra
{
    public class AppDbContext : DbContext
    {
     
        public IConfiguration _configuration { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opt, IConfiguration configuration) : base(opt)
        {
            _configuration = configuration;
        }

        public DbSet<Failure> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FailureModelConfiguration(_configuration));
        }
    }
}
