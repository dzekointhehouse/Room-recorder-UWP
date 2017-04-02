using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace CodebustersAppWMU3.Services
{
    class ErrorMessage
    {
        public static async void DisplayErrorDialog(string message)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(message);
            await dialog.ShowAsync();

        }
    }
}
