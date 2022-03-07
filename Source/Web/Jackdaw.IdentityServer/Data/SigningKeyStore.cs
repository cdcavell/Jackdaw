using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Jackdaw.IdentityServer.Data
{
    /// <summary>
    /// Implimentation of ISigningKeyStore
    /// &lt;br /&gt;&lt;br /&gt;
    /// See https://docs.duendesoftware.com/identityserver/v6/data/operational/keys/ for more information.
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public class SigningKeyStore : ISigningKeyStore
    {
        private readonly ApplicationDbContext _DbContext;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>SigningKeyStore(ApplicationDbContext dbContext)</method>
        public SigningKeyStore(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        /// <summary>
        /// DeleteKeyAsync is used to purge the key from the store.
        /// </summary>
        /// <param name="id">string</param>
        /// <returns>Task</returns>
        /// <method>DeleteKeyAsync(string id)</method>
        public async Task DeleteKeyAsync(string id)
        {
            SerializedKey? key = _DbContext.SerializedKey
                .Where(x => x.Id == id.Clean())
                .FirstOrDefault();
            
            if (key != null)
            {
                _DbContext.SerializedKey.Remove(key);
                await _DbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// LoadKeysAsync is used to load all keys from the store.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;SerializedKey&gt;&gt;</returns>
        /// <method>LoadKeysAsync()</method>
        public async Task<IEnumerable<SerializedKey>> LoadKeysAsync()
        {
            IEnumerable<SerializedKey> keys = _DbContext.SerializedKey;
            return await Task.FromResult(keys);
        }

        /// <summary>
        /// StoreKeyAsync is used to persist the new key.
        /// </summary>
        /// <param name="key">SerializedKey</param>
        /// <returns>Task</returns>
        /// <method>StoreKeyAsync(SerializedKey key)</method>
        public async Task StoreKeyAsync(SerializedKey key)
        {
            int count = _DbContext.SerializedKey
                .Where(x => x == key)
                .Count();

            if (count == 0)
            {
                _DbContext.SerializedKey.Add(key);
                await _DbContext.SaveChangesAsync();
            }
        }
    }
}
