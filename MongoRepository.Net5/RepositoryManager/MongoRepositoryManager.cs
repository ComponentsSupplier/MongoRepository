namespace MongoRepository.Net5
{
    using MongoDB.Driver;
    // using MongoDB.Driver.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // TODO: Code coverage here is near-zero. A new RepoManagerTests.cs class needs to be created and we need to
    //      test these methods. Ofcourse we also need to update codeplex documentation on this entirely new object.
    //      This is a work-in-progress.

    // TODO: GetStats(), Validate(), GetIndexes and EnsureIndexes(IMongoIndexKeys, IMongoIndexOptions) "leak"
    //      MongoDb-specific details. These probably need to get wrapped in MongoRepository specific objects to hide
    //      MongoDb.

    /// <summary>
    /// Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    /// as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    /// objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoRepositoryManager<T, TKey> : IRepositoryManager<T, TKey>
        where T : IEntity<TKey>
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        private IMongoCollection<T> collection;

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepositoryManager()
            : this(Util<TKey>.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString)
        {
            this.collection = Util<TKey>.GetCollectionFromConnectionString<T>(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryManager(string connectionString, string collectionName)
        {
            this.collection = Util<TKey>.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        /// <summary>
        /// Gets a value indicating whether the collection already exists.
        /// </summary>
        /// <value>Returns true when the collection already exists, false otherwise.</value>
        public virtual bool Exists
        {
            get { throw new NotImplementedException(); }
            // get { return this.collection.Exists(); }
        }

        /// <summary>
        /// Gets the name of the collection as Mongo uses.
        /// </summary>
        /// <value>The name of the collection as Mongo uses.</value>
        public virtual string Name
        {
            get { return this.collection.CollectionNamespace.CollectionName; }
        }

        /// <summary>
        /// Drops the collection.
        /// </summary>
        public virtual void Drop()
        {
            this.collection.Database.DropCollection(this.Name);
        }

        /// <summary>
        /// Drops the database.
        /// </summary>
        public virtual void DropDatabase()
        {
            var databaseName = this.collection.Database.DatabaseNamespace.DatabaseName;
            this.collection.Database.Client.DropDatabase(databaseName);
        }

        /// <summary>
        /// Tests whether the repository is capped.
        /// </summary>
        /// <returns>Returns true when the repository is capped, false otherwise.</returns>
        public virtual bool IsCapped()
        {
            throw new NotImplementedException();
            // return this.collection.IsCapped();
        }

        /// <summary>
        /// Drops specified index on the repository.
        /// </summary>
        /// <param name="keyname">The name of the indexed field.</param>
        public virtual void DropIndex(string keyname)
        {
            this.collection.Indexes.DropOne(keyname);
        }

        /// <summary>
        /// Drops specified indexes on the repository.
        /// </summary>
        /// <param name="keynames">The names of the indexed fields.</param>
        public virtual void DropIndexes(IEnumerable<string> keynames)
        {
            foreach(var keyname in keynames)
            {
                this.DropIndex(keyname);
            }
        }

        /// <summary>
        /// Drops all indexes on this repository.
        /// </summary>
        public virtual void DropAllIndexes()
        {
            this.collection.Indexes.DropAll();
        }

        /// <summary>
        /// Gets the indexes for this repository.
        /// </summary>
        /// <returns>Returns the indexes for this repository.</returns>
        public IMongoIndexManager<T> GetIndexes()
        {
            return this.collection.Indexes;
        }
    }

    /// <summary>
    /// Deals with the collections of entities in MongoDb. This class tries to hide as much MongoDb-specific details
    /// as possible but it's not 100% *yet*. It is a very thin wrapper around most methods on MongoDb's MongoCollection
    /// objects.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository to manage.</typeparam>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public class MongoRepositoryManager<T> : MongoRepositoryManager<T, string>, IRepositoryManager<T>
        where T : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepositoryManager()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepositoryManager(string connectionString)
            : base(connectionString) { }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryManager(string connectionString, string collectionName)
            : base(connectionString, collectionName) { }
    }
}
