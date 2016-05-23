using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCore
{
    public interface IUserNotificator
    {
        Task<string> DisplayActionSheet(
            string title, string cancel, string destruction, params string[] buttons);
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}
