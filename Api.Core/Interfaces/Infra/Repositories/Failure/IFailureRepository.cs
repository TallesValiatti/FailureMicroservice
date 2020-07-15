using Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Interfaces.Infra.Repositories.Security
{
    public interface IFailureRepository<T> : IGenerericRepository<T> where T : BaseEntity
    {
        
    }
}
