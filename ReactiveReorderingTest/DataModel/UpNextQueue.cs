using Realms;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveReorderingTest.DataModel
{
    public class UpNextQueue : RealmObject
    {
        public static UpNextQueue Instance
        {
            get
            {
                var r = DataModelManager.RealmInstance;

                var queue = r.All<UpNextQueue>();
                var queueInstance = EnsureQueue(r, queue);
                return queueInstance;
            }
        }

        private static UpNextQueue EnsureQueue(Realm r, IQueryable<UpNextQueue> queue)
        {
            var item = queue.FirstOrDefault();
            if (item == null)
            {
                var trans = r.BeginWrite();
                item = new UpNextQueue();
                r.Add(item);
                trans.Commit();
            }

            return item;
        }

        public Episode CurrentEpisode { get; set; }

        public IList<UpNextQueueEntry> Queue { get; }

        #region Debug
        //private void CreateInitialData()
        //{
        //    var trans = r.BeginWrite();

        //    Feed f = new Feed() { Title = "Windows Weekly", Uri = new Uri("http://feeds.twit.tv/ww.xml").ToString() };
        //    r.Add(f);

        //    Episode e = new Episode() { Title = "WW 531: B is for Broadwell", Feed = f, Descrtiption = "The one where Paul misbehaves", UriKey = "http://www.podtrac.com/pts/redirect.mp3/cdn.twit.tv/audio/ww/ww0531/ww0531.mp3" };
        //    e.PlaybackState = new PlaybackState() { ElapsedTime = 10 };
        //    r.Add(e);

        //    UpNextQueueEntry en = new UpNextQueueEntry() { Episode = e, QuePosition = 0 };
        //    u.Queue.Insert(0, en);

        //    f = new Feed() { Title = "MacBreak Weekly", Uri = new Uri("http://feeds.twit.tv/mbw.xml").ToString() };
        //    r.Add(f);

        //    e = new Episode() { Title = "MBW 571: Cardboard Hats for Dung Beetles", Feed = f, Descrtiption = "The one where Paul misbehaves", UriKey = "http://www.podtrac.com/pts/redirect.mp3/cdn.twit.tv/audio/mbw/mbw0571/mbw0571.mp3" };
        //    r.Add(e);
        //    en = new UpNextQueueEntry() { Episode = e, QuePosition = 1 };
        //    u.Queue.Insert(1, en);

        //    trans.Commit();
        //} 
        #endregion
    }
}
