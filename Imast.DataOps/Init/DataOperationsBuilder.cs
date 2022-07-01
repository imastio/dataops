using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Imast.DataOps.Api;
using Imast.DataOps.Definitions;
using Imast.DataOps.Impl;

namespace Imast.DataOps.Init
{
    /// <summary>
    /// The Data Operations builder
    /// </summary>
    public class DataOperationsBuilder
    {
        /// <summary>
        /// The registry of operations
        /// </summary>
        protected OperationRegistry operations;

        /// <summary>
        /// The connection suppliers registry
        /// </summary>
        protected ConnectionSupplierRegistry suppliers;

        /// <summary>
        /// The default provider
        /// </summary>
        protected SqlProvider? defaultProvider;

        /// <summary>
        /// The default data source
        /// </summary>
        protected string defaultDataSource;

        /// <summary>
        /// The flag to indicate if sources should be validated against schema
        /// </summary>
        protected bool schemaValidation;

        /// <summary>
        /// The set of operation sources
        /// </summary>
        protected List<Func<Stream>> sources;

        /// <summary>
        /// Creates new instance of Data Ops Builder
        /// </summary>
        protected DataOperationsBuilder()
        {
            this.operations = new OperationRegistry();
            this.suppliers = new ConnectionSupplierRegistry();
            this.defaultProvider = null;
            this.schemaValidation = false;
            this.sources = new List<Func<Stream>>();
        }

        /// <summary>
        /// Start new instance of builder
        /// </summary>
        /// <returns></returns>
        public static DataOperationsBuilder New()
        {
            return new DataOperationsBuilder();
        }

        /// <summary>
        /// Use given provider as default for this data operations instance
        /// </summary>
        /// <param name="provider">The target provider</param>
        /// <returns></returns>
        public DataOperationsBuilder WithDefaultProvider(SqlProvider? provider)
        {
            this.defaultProvider = provider;
            return this;
        }

        /// <summary>
        /// Use given data source as default for this data operations instance
        /// </summary>
        /// <param name="dataSource">The target data source</param>
        /// <returns></returns>
        public DataOperationsBuilder WithDefaultDataSource(string dataSource)
        {
            this.defaultDataSource = dataSource;
            return this;
        }

        /// <summary>
        /// Use given configuration to indicate if sources should be validated
        /// </summary>
        /// <param name="validate">The schema validation indicator</param>
        /// <returns></returns>
        public DataOperationsBuilder WithSchemaValidation(bool validate = true)
        {
            this.schemaValidation = validate;
            return this;
        }

        /// <summary>
        /// Use the given source of operation definitions
        /// </summary>
        /// <param name="path">The path to definitions file</param>
        /// <returns></returns>
        public DataOperationsBuilder WithSource(string path)
        {
            this.sources.Add(() => new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            return this;
        }

        /// <summary>
        /// Use the given source of operation definitions
        /// </summary>
        /// <param name="stream">The stream of definitions</param>
        /// <returns></returns>
        public DataOperationsBuilder WithSource(Stream stream)
        {
            this.sources.Add(() => stream);
            return this;
        }

        /// <summary>
        /// Add a connection supplier for the mentioned provider 
        /// </summary>
        /// <param name="provider">The sql provider</param>
        /// <param name="supplier">The connection supplier for the given provider</param>
        /// <returns></returns>
        public DataOperationsBuilder WithConnection(SqlProvider provider, Func<IDbConnection> supplier)
        {
            this.suppliers.Register(provider, supplier);
            return this;
        }

        /// <summary>
        /// Add a connection supplier for the mentioned provider 
        /// </summary>
        /// <param name="dataSource">The data source name</param>
        /// <param name="provider">The sql provider</param>
        /// <param name="supplier">The connection supplier for the given provider</param>
        /// <returns></returns>
        public DataOperationsBuilder WithConnection(string dataSource, SqlProvider provider, Func<IDbConnection> supplier)
        {
            this.suppliers.Register(dataSource, provider, supplier);
            return this;
        }

        /// <summary>
        /// Build Data Operations instance from pieces
        /// </summary>
        /// <returns></returns>
        public DataOperations Build()
        {
            // create a parser instance 
            var parser = new DataOperationsParser(this.schemaValidation);

            // process sources to build operations registry
            this.sources.ForEach(source =>
            {
                // invoke source to get stream
                using var stream = source.Invoke();

                // parse stream and get result
                var result = parser.Parse(stream);

                // process definitions
                this.ProcessDefinition(result);
            });

            if(this.defaultProvider == null)
            {
                throw new InvalidOperationException("The default provider is required to be given");
            }

            return new DataOperations(this.operations, this.suppliers, this.defaultDataSource, this.defaultProvider.Value);
        }

        /// <summary>
        /// Process definition to extract all required information
        /// </summary>
        /// <param name="definition">The definition instance</param>
        private void ProcessDefinition(DataOperationsDefinition definition)
        {
            // get configuration
            var config = definition.Configuration ?? new DataConfiguration();

            // process every group
            definition.Groups.ForEach(grp =>
            {
                // process every definition
                grp.Operations.ForEach(op =>
                {
                    // the key of operation
                    var opKey = OpKey.Of(op.Group.Name, op.Name);

                    // the auto-transaction setup
                    var autoTransaction = op.AutoTransaction ?? op.Group.AutoTransaction ?? config.AutoTransaction;

                    // the timeout setup value
                    var timeout = op.Timeout ?? op.Group.Timeout ?? config.Timeout;

                    // the expected result type
                    var resultType = op.Command?.ExpectedResult switch
                    {
                        ExpectedResult.Table => ResultType.Table,
                        ExpectedResult.MultipleTables => ResultType.MultipleTables,
                        ExpectedResult.Scalar => ResultType.Scalar,
                        ExpectedResult.RowCount => ResultType.RowCount,
                        ExpectedResult.Unknown => ResultType.Unknown,
                        null => ResultType.Unknown,
                        _ => ResultType.Unknown
                    };

                    // the target operation type
                    var operationType = op.Command?.Type switch
                    {
                        CommandTypeOption.Text => OperationType.Text,
                        CommandTypeOption.StoredProcedure => OperationType.StoredProcedure,
                        CommandTypeOption.BulkInsert => OperationType.BulkInsert,
                        CommandTypeOption.Unknown => OperationType.Unknown,
                        null => OperationType.Unknown,
                        _ => OperationType.Unknown
                    };

                    // the source of operation
                    var source = op.Command?.Source ?? string.Empty;

                    // get the compatibility providers
                    var compatibility = op.Providers ?? CompatibilityProviders.Any;

                    // get supported providers
                    var providers = GetEnumValues<SqlProvider>().Where(p => compatibility.HasFlag(MapProvider(p)));

                    // the target operation
                    var operation = new SqlOperation
                    {
                        Name = opKey.Name,
                        Group = opKey.Group,
                        AutoTransaction = MapAutoTransaction(autoTransaction),
                        Timeout = timeout,
                        ResultType = resultType,
                        Type = operationType,
                        Source = source
                    };

                    // add operation to every supported provider
                    foreach (var provider in providers)
                    {
                        this.operations.Register(opKey, provider, operation);
                    }
                });
            });
        }

        /// <summary>
        /// Maps the target provider into compatibility flag
        /// </summary>
        /// <param name="provider">The provider to map</param>
        /// <returns></returns>
        protected static CompatibilityProviders MapProvider(SqlProvider provider)
        {
            return provider switch
            {
                SqlProvider.SqlServer => CompatibilityProviders.SqlServer,
                SqlProvider.MySQL => CompatibilityProviders.SqlServer,
                SqlProvider.PostgreSQL => CompatibilityProviders.PostgreSQL,
                SqlProvider.SQLite => CompatibilityProviders.SQLite,
                _ => CompatibilityProviders.None
            };
        }

        /// <summary>
        /// Maps defined auto-transaction mode for the operation
        /// </summary>
        /// <param name="autoTransaction">The defined auto-transaction</param>
        /// <returns></returns>
        protected static AutoTransaction? MapAutoTransaction(AutoTransactionMode? autoTransaction)
        {
            // no auto-transaction is setup
            if (autoTransaction == null)
            {
                return null;
            }

            return autoTransaction.Value switch
            {
                AutoTransactionMode.No => null,
                AutoTransactionMode.RepeatableRead => AutoTransaction.RepeatableRead,
                AutoTransactionMode.ReadUncommitted => AutoTransaction.ReadUncommitted,
                AutoTransactionMode.ReadCommitted => AutoTransaction.ReadCommitted,
                AutoTransactionMode.Serializable => AutoTransaction.Serializable,
                _ => null
            };
        }

        /// <summary>
        /// Gets the enum values of the given type
        /// </summary>
        /// <typeparam name="TValue">The enum value type</typeparam>
        /// <returns></returns>
        private static TValue[] GetEnumValues<TValue>() where TValue: Enum {
            return Enum.GetValues(typeof(TValue)).Cast<TValue>().ToArray();
        }
    }
}