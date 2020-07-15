using Api.Core.Entities.Failure;
using Api.Core.Interfaces.Infra.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infra.Repositories.Security
{
    public class FailureRepository : GenericRepository<Failure>, IFailureRepository<Failure> 
    {
        public FailureRepository(AppDbContext context) : base(context)
        {}

    }
}
