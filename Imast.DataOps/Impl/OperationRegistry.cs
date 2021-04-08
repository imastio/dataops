using System.Collections.Generic;
using Imast.DataOps.Api;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The Sql Operation registry
    /// </summary>
    public class OperationRegistry : IOperationRegistry
    {
        /// <summary>
        /// The operations identifier by OpKey -> Provider -> Operation
        /// </summary>
        protected readonly Dictionary<OpKey, Dictionary<SqlProvider, SqlOperation>> operations;

        /// <summary>
        /// Creates new 
        /// </summary>
        public OperationRegistry()
        {
            this.operations = new Dictionary<OpKey, Dictionary<SqlProvider, SqlOperation>>();
        }

        /// <summary>
        /// Registers the operation with key and provider
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <param name="provider">The provider of operation</param>
        /// <param name="operation">The operation itself</param>
        public void Register(OpKey key, SqlProvider provider, SqlOperation operation)
        {
            // if key is not known within registry register it
            if (!this.operations.TryGetValue(key, out var providerOps))
            {
                // create new collection
                providerOps = new Dictionary<SqlProvider, SqlOperation>();
                
                // add to operations for given key
                this.operations.Add(key, providerOps);
            }

            // override or add operation for the provider
            providerOps[provider] = operation;
        }

        /// <summary>
        /// Checks if operation is defined for at least one provider
        /// </summary>
        /// <param name="key">The operation key</param>
        /// <returns></returns>
        public bool IsDefined(OpKey key)
        {
            // check if operation key exists and has at least one item inside
            if (this.operations.TryGetValue(key, out var ops))
            {
                return ops.Count != 0;
            }

            // not defined
            return false;
        }
        
        /// <summary>
        /// Try get operation for given key and provider and null otherwise
        /// </summary>
        /// <param name="key">The key of operation</param>
        /// <param name="provider">The provider</param>
        /// <returns></returns>
        public SqlOperation GetOrNull(OpKey key, SqlProvider provider)
        {
            // operation is not defined 
            if (!this.operations.TryGetValue(key, out var providerOps))
            {
                return null;
            }

            // operation is defined but not available for the provider
            return providerOps.TryGetValue(provider, out var operation) ? operation : null;
        }
    }
}