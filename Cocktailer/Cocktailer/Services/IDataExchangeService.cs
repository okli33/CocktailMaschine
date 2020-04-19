using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface IDataExchangeService
    {
        Task Export();
        Task Import();
    }
}
