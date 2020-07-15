using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Entities.Failure
{
    public class Failure : BaseEntity
    {
        public String Body { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
