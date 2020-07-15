using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Interfaces.Core.Entities
{
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}
