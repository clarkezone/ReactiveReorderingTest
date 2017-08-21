using Realms;

namespace ReactiveReorderingTest.DataModel
{
    public class Episode : RealmObject
    {
        [PrimaryKey]
        public string UriKey { get; set; }

        public string Title { get; set; }

        public string Descrtiption { get; set; }

        public PlaybackState PlaybackState { get; set; }

        public Feed Feed { get; set; }
    }
}
