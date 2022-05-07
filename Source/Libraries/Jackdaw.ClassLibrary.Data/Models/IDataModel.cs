namespace Jackdaw.ClassLibrary.Data.Models
{
    /// <summary>
    /// DataModel Interface
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 05/07/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public interface IDataModel<T>
    {
        Guid Guid { get; set; }

        bool IsNew { get; }

        /// <summary>
        /// Method used to add new or update existing entity record.
        /// </summary>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>AddUpdate(ApplicationDbContext dbContext)</method>
        void AddUpdate(ApplicationDbContext dbContext);
        /// <summary>
        /// Method used to remove existing entity record.
        /// </summary>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>Delete(ApplicationDbContext dbContext)</method>
        void Delete(ApplicationDbContext dbContext);
        /// <summary>
        /// Equality method.
        /// </summary>
        /// <param name="obj">DataModel&lt;T&gt;</param>
        /// <method>Equals(DataModel&lt;T&gt; obj)</method>
        bool Equals(T obj);
    }
}
