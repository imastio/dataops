namespace Imast.DataOps.Cli.Model
{
    /// <summary>
    /// The Northwind product structure  
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The supplier id
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// The category id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The quantity per unit
        /// </summary>
        public string QuantityPerUnit { get; set; }

        /// <summary>
        /// The unit price
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// The number of units in stock
        /// </summary>
        public int UnitsInStock { get; set; }

        /// <summary>
        /// The number of units in order
        /// </summary>
        public int UnitsOnOrder { get; set; }

        /// <summary>
        /// The level of reorder
        /// </summary>
        public int ReorderLevel { get; set; }

        /// <summary>
        /// The discontinue indicator
        /// </summary>
        public bool Discontinued { get; set; }
    }
}