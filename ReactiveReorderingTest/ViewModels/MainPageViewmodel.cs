using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using ReactiveReorderingTest.DataModel;
using ReactiveReorderingTest.Services;
using ReactiveReorderingTest.UWP.XAML;
using Realms;
using System.Diagnostics;
using System.Windows.Input;

namespace ReactiveReorderingTest.ViewModels
{
    public class MainPageViewmodel : ViewModelBase
    {
        Realm r = null;

        public UpNextQueue UpNext { get; private set; }

        public RealmVirtualOrderedUpNextEntrySource VirtualItemSource { get; private set; }

        public RealmAllEpisodeSource VirtualAllEpisodeSource { get; private set; }

        #region DebugStuff
        // This is used to test how observablecollection implements INotifyCollectionChanged, System.Collections.IList, IItemsRangeInfo, ISelectionInfo
        //public ObservableCollection<UpNextQueueEntry> NonVirtualItemSource { get; private set; } 
        #endregion

        public MainPageViewmodel()
        {
            this.UpNext = UpNextQueue.Instance;

            #region DebugStuff
            this.UpNext.CurrentEpisode.PlaybackState.PropertyChanged += (o, ex) =>
                {
                    Debug.WriteLine("State changed");
                    RaisePropertyChanged("UpNext");
                };

            this.UpNext.CurrentEpisode.PropertyChanged += (o, ex) =>
            {
                Debug.WriteLine("CurrentEpisode changed");
            };

            this.UpNext.PropertyChanged += (o, ex) =>
            {
                Debug.WriteLine("Upnext changed");
            }; 
            #endregion

            RaisePropertyChanged("UpNext");

            VirtualItemSource = new RealmVirtualOrderedUpNextEntrySource(this.UpNext);

            VirtualAllEpisodeSource = new RealmAllEpisodeSource();

            #region DebugStuff
            //used to test observablecolleciton behavior
            //NonVirtualItemSource = new ObservableCollection<UpNextQueueEntry>();
            //NonVirtualItemSource.CollectionChanged += NonVirtualItemSource_CollectionChanged;

            //foreach (var item in VirtualItemSource)
            //{
            //    NonVirtualItemSource.Add((UpNextQueueEntry)item);
            //} 
            #endregion

            PlaybackCommand = new RelayCommand(() =>
            {
                var service = SimpleIoc.Default.GetInstance<IFakeMediaplayerService>();
                service.Play();
            });

            StopCommand = new RelayCommand(() =>
            {
                var service = SimpleIoc.Default.GetInstance<IFakeMediaplayerService>();
                service.Stop();
            });
        }

        #region DebugStuff
        //private void NonVirtualItemSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    Debug.WriteLine(sender, e.Action.ToString());
        //} 
        #endregion

        internal void EndReorder()
        {
            VirtualItemSource.EndReorder();
        }

        internal void BeginReorder()
        {
            VirtualItemSource.BeginReorder();
        }

        public ICommand PlaybackCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

       
    }
}
