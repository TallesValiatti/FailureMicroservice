using Api.Core.Interfaces.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
namespace Api.Core.Entities
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
