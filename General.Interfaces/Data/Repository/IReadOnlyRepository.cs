using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Data.Repository
{
    public interface IReadOnlyRepository<TDataItem> 
        where TDataItem : class
    {        
        IReadOnlyList<TDataItem> Read(out bool flag);       
    }
}
