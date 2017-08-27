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
        private new RelayCommand<EpisodeViewModel> _incrementCommand;

        public DataModel.Episode Episode { get; set; }

        public ICommand AddUpNext { get; set; }

        public new RelayCommand<EpisodeViewModel> AddItemUpNextCommand
        {
            get
            {
                return _incrementCommand
                    ?? (_incrementCommand = new RelayCommand<EpisodeViewModel>(
                    (m) =>
                    {
                        var trans = DataModelManager.RealmInstance.BeginWrite();

                        var upNextQueue = DataModelManager.RealmInstance.All<UpNextQueue>().FirstOrDefault();
                        
                        UpNextQueueEntry en = new UpNextQueueEntry() { Episode = m.Episode, QuePosition = upNextQueue.Queue.Count };
                        upNextQueue.Queue.Insert(upNextQueue.Queue.Count, en);

                        trans.Commit();

                        
                    }));
            }
        }
    }
}
