using Cocktailer.Models.Entries;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface ILogService
    {
        Task AddToLogFile(LogEntry entry);
        Task ShareLogFile(IFile logFile);
        Task<List<IFile>> GetLogFiles();
    }
}
