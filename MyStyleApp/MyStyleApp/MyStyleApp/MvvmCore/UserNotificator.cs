using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MvvmCore
{
    public class UserNotificator : IUserNotificator
    {
        private readonly IDeviceService _deviceService;

        public UserNotificator(IDeviceService deviceService)
        {
            this._deviceService = deviceService;
        }

        public Task<string> DisplayActionSheet(
            string title, string cancel, string destruction, params string[] buttons)
        {
            var tcs = new TaskCompletionSource<string>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                var result = await Application.Current.MainPage.DisplayActionSheet(
                    title, cancel, destruction, buttons);
                tcs.SetResult(result);
            });
            return tcs.Task;
        }

        public Task DisplayAlert(
            string title, string message, string cancel)
        {
            var tcs = new TaskCompletionSource<object>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(
                    title, message, cancel);
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        public Task<bool> DisplayAlert(
            string title, string message, string accept, string cancel)
        {
            var tcs = new TaskCompletionSource<bool>();
            _deviceService.BeginInvokeOnMainThread(async () =>
            {
                var result = await Application.Current.MainPage.DisplayAlert(
                    title, message, accept, cancel);
                tcs.SetResult(result);
            });
            return tcs.Task;
        }
    }
}
