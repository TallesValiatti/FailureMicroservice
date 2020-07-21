using Api.Core.Entities.Failure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Infra.ModelsConfiguration
{
    class FailureModelConfiguration : IEntityTypeConfiguration<Failure>
    {
        public IConfiguration _configuration{ get; set; }
        public FailureModelConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(EntityTypeBuilder<Failure> builder)
        {
            builder.ToTable(nameof(Failure));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Body)
                .IsRequired();

            builder.Property(x => x.CreateAt)
              .IsRequired();
        }
    }
}
