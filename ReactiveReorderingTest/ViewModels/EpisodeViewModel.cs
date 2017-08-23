using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReactiveReorderingTest.DataModel;
using Realms;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ReactiveReorderingTest.ViewModels
{
    class EpisodeViewModel : ViewModelBase
    {
        private RelayCommand _incrementCommand;

        public DataModel.Episode Episode { get; set; }

        public ICommand AddUpNext { get; set; }

        public RelayCommand AddItemUpNextCommand
        {
            get
            {
                return _incrementCommand
                    ?? (_incrementCommand = new RelayCommand(
                    () =>
                    {
                        var upNextQueue = DataModelManager.RealmInstance.All<UpNextQueue>().FirstOrDefault();
                        

                        // UpNextQueueEntry en = new UpNextQueueEntry() { Episode = e, QuePosition = 0 };
                        //u.Queue.Insert(0, en);
                    }));
            }
        }
    }
}
