using HaxOnTheWay.Helpers;
using HaxOnTheWay.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HaxOnTheWay.ViewModels
{
    class CommandsPageViewModel : ViewModelBase
    {
        #region Commands
        public INavigation Navigation { get; set; }
        public ICommand LogoutCommand { get; set; }
        #endregion

        public CommandsPageViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        
    }
}
