using Realms;

namespace ReactiveReorderingTest.DataModel
{
    public class UpNextQueueEntry : RealmObject
    {
        public long QuePosition { get; set; }

        public Episode Episode { get; set; }
    }
}
