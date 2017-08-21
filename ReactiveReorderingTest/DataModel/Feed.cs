using Realms;

namespace ReactiveReorderingTest.DataModel
{
    public class Feed : RealmObject
    {
        public string Title { get; set; }

        [PrimaryKey]
        public string Uri { get; set; }
    }
}
