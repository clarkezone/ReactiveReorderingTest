using Realms;
using System.Collections.Generic;

namespace ReactiveReorderingTest.DataModel
{
    public class UpNextQueue : RealmObject
    {
        public Episode CurrentEpisode { get; set; }

        public IList<UpNextQueueEntry> Queue { get; }
    }
}
