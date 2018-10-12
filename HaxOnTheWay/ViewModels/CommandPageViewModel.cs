using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HaxOnTheWay.ViewModels
{
    class CommandPageViewModel : ViewModelBase
    {
        public CommandPageViewModel() {
            CommandsCommand = new Command(Commands);
            LogoutCommand = new Command(Logout);
        }

        public Command CommandsCommand { get; set; }
        public Command LogoutCommand { get; set; }
    }
}
