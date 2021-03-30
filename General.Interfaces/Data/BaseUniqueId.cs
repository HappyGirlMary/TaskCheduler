using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Data
{
    public class BaseUniqueId<TUniqueId>
        where TUniqueId : struct
    {
        public TUniqueId Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
