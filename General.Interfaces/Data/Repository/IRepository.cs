using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Data.Repository
{
    public interface IRepository <TDataItem, TUniqueId> : IReadOnlyRepository<TDataItem> 
        where TDataItem : BaseUniqueId<TUniqueId >
        where TUniqueId : struct
    {
        void Create(TDataItem data);

        void Update(TDataItem data);

        void Delete(TUniqueId d);
    }
}
