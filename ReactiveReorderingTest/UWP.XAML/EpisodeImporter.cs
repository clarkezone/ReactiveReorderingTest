using ObjectivePixel.BringCast.DataModel.DataAccess;
using ObjectivePixel.Flame;
using ObjectivePixel.Flame.Download;
using ObjectivePixel.Flame.SQLite;
using Realms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveReorderingTest.UWP.XAML.Import
{
    class NullPlatformAdapter : PlatformAdapter
    {
        public override ICryptoProvider CryptoProvider => throw new NotImplementedException();

        public override IDispatcher Dispatcher => throw new NotImplementedException();

        public override IQueue DownloadQueue => throw new NotImplementedException();

        public override IObjectFactory ObjectFactory => throw new NotImplementedException();

        public override StorageBase Storage => new NullStorage();

        public override IThreading Threading => throw new NotImplementedException();

        public override IWatson Watson => throw new NotImplementedException();
    }

    class NullStorage : StorageBase
    {
        public NullStorage()
        {

        }
        public override Task CopyFileAsync(string sourceFileName, string destinationFileName)
        {
            throw new NotImplementedException();
        }

        public override Task CreateDirectoryIfNotExistsAsync(string directoryName)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteDirectoryAsync(string directoryName)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteFileAsync(string directory, string fileName)
        {
            throw new NotImplementedException();
        }

        public override Task<ulong> FileExistLengthAsync(string path)
        {
            throw new NotImplementedException();
        }

        public override Task<ulong?> FileExistLengthAsync(string directory, string filename)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> FileExistsAsync(string path)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> FolderExistsAndHasContents(string path)
        {
            throw new NotImplementedException();
        }

        public override Task<Stream> GetReadOnlyStreamToFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public override Task<Stream> GetReadOnlyStreamToFileAsync(string directory, string fileName)
        {
            throw new NotImplementedException();
        }

        public override Task<Stream> GetReadWriteStreamToFileAsync(string fileName)
        {
            TaskCompletionSource<Stream> s = new TaskCompletionSource<Stream>();
            s.SetResult(new MemoryStream());
            return s.Task;
        }

        public override Task MoveFileAsync(string sourceFileName, string destinationFileName)
        {
            throw new NotImplementedException();
        }
    }

    class EpisodeImporter
    {
        public async static void Import()
        {
            var r = Realm.GetInstance("testdb");

            Database db = new Database("bringcastdb");
            PlatformAdapter.SetPlatformAdapter(new NullPlatformAdapter(), "Importer");
            FeedRepository feedRepo = new FeedRepository(db);
            var FeedItemRepository = new FeedItemRepository(db);

            var first = feedRepo.GetSubscribed().FirstOrDefault();
            var items = await FeedItemRepository.GetFeedItemsAsync(first);

            var trans = r.BeginWrite();

            var f = new DataModel.Feed() { Title = first.Title, Uri = first.Uri.ToString() };
            r.Add(f);

            
            foreach (var item in items)
            {
                var e = new DataModel.Episode() { Title = item.Title, Feed = f, Descrtiption = item.Description, UriKey = item.Key.ToString().ToLower() };
                r.Add(e);
            }

            trans.Commit();
        }
    }
}
