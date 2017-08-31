using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveReorderingTest.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private new RelayCommand _settingsCommand;

        public NavigationViewModel()
        {
         
        }

        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand
                    ?? (_settingsCommand = new RelayCommand(
                    () =>
                    {
                      


                    }));
            }
        }
    }
}
