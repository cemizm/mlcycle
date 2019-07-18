using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Backend.DataLayer
{
    public interface IBaseService<T> where T : BaseModel
    {
        /// <summary>
        /// Gets all objects of type <typeparamref name="T">
        /// </summary>
        /// <returns>List with objects of type T</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Gets an object of type <typeparamref name="T"> with the given <paramref name="id">
        /// </summary>
        /// <param name="id">The id of the Object to retrive</param>
        /// <returns>The object of type <typeparamref name="T"> for given <paramref name="id"> </returns>
        Task<T> GetById(Guid id);

        Task<List<T>> GetBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Creates a new object of type <typeparamref name="T">
        /// </summary>
        /// <param name="model">object of type <typeparamref name="T"> to create</param>
        Task Add(T model);

        /// <summary>
        /// Updates an object of type <typeparamref name="T">
        /// </summary>
        /// <param name="model">Object of type <typeparamref name="T"> to update</param>
        Task Update(Guid id, T model);

        /// <summary>
        /// Deletes an object with given id
        /// </summary>
        /// <param name="id">Id of the object to delete</param>
        Task Delete(Guid id);
    }
}