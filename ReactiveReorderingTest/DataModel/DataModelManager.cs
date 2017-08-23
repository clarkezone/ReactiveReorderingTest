using Realms;

namespace ReactiveReorderingTest.DataModel
{
    static class DataModelManager
    {
        const string REALMDBNAME = "testdb";

        public static Realm RealmInstance { get {
                return Realm.GetInstance(REALMDBNAME);
            }
        }
    }
}
