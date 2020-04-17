using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cocktailer.Services
{
    public interface IAlertMessageService
    {
        Task ShowErrorMessage(string message);
        Task ShowAlertMessage(string message);
        Task ShowSuccessMessage(string message);
    }
}
