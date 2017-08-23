using ReactiveReorderingTest.DataModel;
using Realms;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveReorderingTest.Services
{
    class FakeMediaplayerService : IFakeMediaplayerService
    {
        private bool playing = false;

        public FakeMediaplayerService()
        {
            //TODO: take a realm and marshal to the bg thread
        }

        public void Play()
        {
            playing = true;

            #region UpdateFromUI
            // Used for testing updating items from UI thread
            //var r = Realm.GetInstance("testdb");
            //var d = r.All<UpNextQueue>().FirstOrDefault();

            //var tx = r.BeginWrite();
            //if (d.CurrentEpisode.PlaybackState == null)
            //{
            //    d.CurrentEpisode.PlaybackState = new PlaybackState();
            //}

            //d.CurrentEpisode.PlaybackState.ElapsedTime = 2000;
            //tx.Commit(); 
            #endregion

            var task = Windows.System.Threading.ThreadPool.RunAsync((s) =>
            {
                var threadsRealm = DataModelManager.RealmInstance;
                var data = threadsRealm.All<UpNextQueue>().FirstOrDefault();

                while (playing && data != null)
                {
                    var tx = threadsRealm.BeginWrite();
                    if (data.CurrentEpisode.PlaybackState == null)
                    {
                        data.CurrentEpisode.PlaybackState = new PlaybackState();
                    }

                    data.CurrentEpisode.PlaybackState.ElapsedTime += 1;
                    tx.Commit();

                    Task.Delay(1000).Wait();
                }
            });
        }

        public void Stop()
        {
            playing = false;
        }
    }
}
