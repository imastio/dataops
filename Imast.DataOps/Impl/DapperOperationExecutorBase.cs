using System.Data;
using System.Text.RegularExpressions;
using Imast.DataOps.Api;
using Imast.DataOps.Definitions;

namespace Imast.DataOps.Impl
{
    /// <summary>
    /// The dapper-based operation executor base
    /// </summary>
    /// <typeparam name="TExecutor">The executor base</typeparam>
    public abstract class DapperOperationExecutorBase<TExecutor> : DapperExecutorBase<TExecutor>
    {
        /// <summary>
        /// The if/else binding regular expression
        /// </summary>
        private static readonly Regex IF_ELSE_REGEX = new Regex(@"{\s*if:(?<binding>[^{}]*)\s*{\s*(?<if>[^{}]*)\s*}\s*else?\s*{\s*(?<else>[^{}]*)\s*}\s*}");
        
        /// <summary>
        /// The operation to execute
        /// </summary>
        public SqlOperation Operation { get; }

        /// <summary>
        /// Creates new instance of dapper-based operation executor base
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="provider">The provider</param>
        /// <param name="operation">The operation to execute</param>
        protected DapperOperationExecutorBase(IDbConnection connection, SqlProvider provider, SqlOperation operation) :
            base(connection, provider, operation.AutoTransaction, operation.Timeout)
        {
            this.Operation = operation;
        }

        /// <summary>
        /// Gets the effective timeout value
        /// </summary>
        /// <returns></returns>
        protected virtual int? GetEffectiveTimeout()
        {
            // the timeout to use
            return this.Timeout.HasValue ? (int)this.Timeout.Value.TotalMilliseconds : default(int?);
        }

        /// <summary>
        /// Gets the effective source value
        /// </summary>
        /// <returns></returns>
        protected virtual string GetEffectiveSource()
        {
            // the source of query
            var source = this.Operation.Source?.ToString() ?? string.Empty;

            // process source in case of text for resolving any bindings
            return this.Operation.Type is OperationType.Text or OperationType.Unknown ? this.ResolveBindings(source) : source;
        }

        /// <summary>
        /// Resolve any source bindings if given
        /// </summary>
        /// <param name="source">The source text</param>
        /// <returns></returns>
        private string ResolveBindings(string source)
        {
            return IF_ELSE_REGEX.Replace(source, (match) =>
            {
                // try get binding name
                var binding = match.Groups["binding"].Value.Trim();

                // leave untouched if binding is found but not specified
                if (string.IsNullOrWhiteSpace(binding) || !this.Bindings.TryGetValue(binding, out var bindingValue))
                {
                    return match.Value;
                }

                // binding is fine if given
                var evaluateBinding = bindingValue != null;

                // if binding is boolean use it as final value
                if (bindingValue is bool boolBinding)
                {
                    evaluateBinding = boolBinding;
                }
                
                return evaluateBinding ? $" {match.Groups["if"].Value.Trim()} " : $" {match.Groups["else"].Value.Trim()} ";
            });
        }

        /// <summary>
        /// Gets the effective command type
        /// </summary>
        /// <returns></returns>
        protected virtual CommandType GetEffectiveCommandType()
        {
            // use type based on value
            return this.Operation.Type == OperationType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
        }
    }
}