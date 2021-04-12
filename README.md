# DataOps 
A lightweight data access library for clean, easy and fast access to any relational database. 

## Why DataOps? 
DataOps provides a declarative way for defining any SQL operations: queries, insert/update/delete operations, stored procedures, custom operations. The library uses Dapper under the hood, which enables high performace mapping functionality. 

Main features:
- XML-based operation definition
- Transactional execution support: Auto-transaction and BYO transactions mode
- Operation, Group, or file-level configuration
- Both provider-specific and generic operations
- Fully asynchronous

## How to use it? 

First, define your operations in an XML file for any SQL provider (example Samples/NorthwindOperations.xml):
```xml
<?xml version="1.0" encoding="utf-8" ?>

<DataOperations xmlns="" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="../../Imast.DataOps/DataOps.xsd">
	
	<!-- The configuration applies to any operation in this file. Using global timeout for 3 minutes -->
	<DataConfiguration Timeout="PT3M" />

	<!-- Defining a group of operations "Products" and requesting any operation inside to have specified timeout -->
	<OperationGroup Name="Products" Timeout="PT5M">
		
		<!-- Defining simple SQL operation which is required to run in a transaction with ReadCommitted Isolation Level   -->
		<SqlOperation Name="GetAll" Compatibility="Any" AutoTransaction="ReadCommitted">

			<!-- A plain text SQL expression, Stored Procedure or any other source of commands -->
			<Command Type="Text">
				select * from Products;
			</Command>
		</SqlOperation>
	</OperationGroup>

	<!-- A group for other operations -->
	<OperationGroup Name="Other">

		<!-- Defining an operation that is compatible only with SQL Server with specified timeout and auto-transaction -->
		<SqlOperation Name="WriteLog" Compatibility="SqlServer" Timeout="PT5M" AutoTransaction="RepeatableRead">
			<Command>
				insert into Log (Message)
				values (@Message)
			</Command>
		</SqlOperation>

	</OperationGroup>
</DataOperations>

```

Next, define, initialize and register a Data Operations entry point as follows:
```cs
// the connection string to Northwind database
var northwindConnectionString = @"Server = (local); Initial Catalog = Northwind; Integrated Security = true;";

// build new data operations
var dataOps = DataOperationsBuilder.New()
    .WithConnection(SqlProvider.SqlServer, () => new SqlConnection(northwindConnectionString))
    .WithDefaultProvider(SqlProvider.SqlServer)
    .WithSchemaValidation()
    .WithSource("Samples/NorthwindOperations.xml")
    .Build();
```

Here you can see and instance of DataOperations is defined with supports SQL Server by default (altough later can be used for any other managed IDbConnection), then we assert source files to be validated agains XSD and, finally, we provide source files.

Then, as an option, the instance to Data Operations can be registered as a service in .Net Core and used like:
```cs
var single = dataOps.Connect().Query("Products", "GetAll").ExecuteAsync<Product>();
```

Method Connect supports both auto-connecting and binding to pre-existing IDbConnection for any SQL Provider.
Before executing, query can be configured (specifying a custom transaction, timeout, other).

For insert/update/delete queries you can use NonQuery operation as follows:
```cs
var writeResult = dataOps.Connect().NonQuery("Other", "WriteLog").ExecuteAsync(new
{
    Message = $"Hello. It's {DateTime.Now}"
})
```

Queries with multiple results can be executed like this:
```cs
// running operation that returns multiple result sets
var multiResult = dataOps.Connect().MultiQuery("Products", "GetProductsAndCategories").ExecuteAsync(async reader =>
{
    var products = await reader.ReadAsync<Product>();
    var cats = await reader.ReadAsync<dynamic>();

    return Tuple.Create(products, cats);
});
```
