using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace Backend.DataLayer.MongoDB
{
    public abstract class BaseService<T> where T : BaseModel
    {
        private readonly MongoContext context;
        private IGridFSBucket bucket;

        public BaseService(MongoSettings settings) 
        {
            context = new MongoContext(settings);
            bucket = context.GetBucket<T>();
        }

        protected IMongoCollection<T> Collection { get { return context.GetCollection<T>(); } }

        protected IGridFSBucket Bucket { get { return bucket; } }

        public Task Add(T model) => Collection.InsertOneAsync(model);

        public Task Delete(Guid id) => Collection.DeleteOneAsync(m => m.Id == id);

        public Task<List<T>> GetAll() => Collection.Find(m => true).ToListAsync();

        public Task<T> GetById(Guid id) => Collection.Find(m => m.Id == id).FirstOrDefaultAsync();

        public Task<List<T>> GetBy(Expression<Func<T, bool>> predicate) => Collection.AsQueryable().Where(predicate).ToListAsync();

        public Task Update(Guid id, T model) => Collection.ReplaceOneAsync(m => m.Id == id, model);
    }
}