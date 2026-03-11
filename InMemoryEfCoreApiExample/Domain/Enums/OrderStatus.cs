using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public class OrderStatus : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
