namespace MongoRepository.NetCore
{
    using System.Collections.Generic;
    using MongoDB.Driver;
    using System;

    /// <summary>
    /// IRepositoryManager definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IRepositoryManager<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        bool Exists { get; }

        /// <summary>
        /// Gets the name of the collection as Mongo uses.
        /// </summary>
        /// <value>The name of the collection as Mongo uses.</value>
        string Name { get; }

        /// <summary>
        /// Drops the repository.
        /// </summary>
        void Drop();

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        bool IsCapped();

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        void DropIndex(string keyname);

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        void DropIndexes(IEnumerable<string> keynames);

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        void DropAllIndexes();

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <returns>Returns the indexes for this repository.</returns>
        IMongoIndexManager<T> GetIndexes();
    }

    /// <summary>
    /// IRepositoryManager definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IRepositoryManager<T> : IRepositoryManager<T, string>
        where T : IEntity<string> { }
}
