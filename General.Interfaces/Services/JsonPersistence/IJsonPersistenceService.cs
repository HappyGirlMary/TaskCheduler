using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Services.JsonPersistence
{
    public interface IJsonPersistenceService
    {
        string Serialize<TItem>(TItem item);

        TItem Deserialize<TItem>(string jsonBody);
    }
}
