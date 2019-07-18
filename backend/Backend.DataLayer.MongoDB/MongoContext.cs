using Backend.DataLayer.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Backend.DataLayer.MongoDB
{
    internal class MongoContext
    {
        private readonly IMongoDatabase db;

        static MongoContext()
        {
            BsonClassMap.RegisterClassMap<BaseModel>(cm => {
                cm.AutoMap();
                cm.MapMember(c => c.Id).SetIgnoreIfDefault(true);
            });
            BsonClassMap.RegisterClassMap<Worker>(cm => {
                cm.AutoMap();
            });
            BsonClassMap.RegisterClassMap<Project>(cm => {
                cm.AutoMap();
            });
            BsonClassMap.RegisterClassMap<Job>(cm => {
                cm.AutoMap();
            });
            BsonClassMap.RegisterClassMap<Step>(cm => {
                cm.AutoMap();
            });
            BsonClassMap.RegisterClassMap<Fragment>(cm => {
                cm.AutoMap();
            });
        }

        public MongoContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            db = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<T> GetCollection<T>() => db.GetCollection<T>(typeof(T).Name);
        
        public IGridFSBucket GetBucket<T>() => new GridFSBucket(db, new GridFSBucketOptions { BucketName=typeof(T).Name });

    }
}