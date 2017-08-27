using Realms;
using System.Threading;

namespace ReactiveReorderingTest.DataModel
{
    static class DataModelManager
    {
        const string REALMDBNAME = "testdb";
        static ThreadLocal<Realm> instance;

        public static Realm RealmInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThreadLocal<Realm>();
                }

                if (instance.Value==null)
                {
                    instance.Value = Realm.GetInstance(REALMDBNAME);
                }

                return instance.Value;
            }
        }
    }
}
