
# Script #

## Introduction ##

Walk-through an example of a Web API the performs CRUD operations using the CosmosDB 2.0 and 3.0 SDK, the latter currently in preview.
- The 3.0 SDK unifies the .NET and .NET Core SDKs into a single .NET SDK,
- which targets .NET Standard 2.0.
- This means you can now use the .NET SDK in both your .NET Framework 4.6.1+ and .NET Core 2.0+ applications.

For more information on the CosmosDB 3.0 SDK see
https://azure.microsoft.com/en-us/blog/azure-cosmos-dotnet-sdk-version-3-0-now-in-public-preview/

Show Swagger at https://localhost:44346/swagger/index.html.

## Setup the Environment ##

Navigate to https://shell.azure.com/

- az group create --name marumacosmosdbwebapi --location eastus2

- az cosmosdb create --name marumacosmosdbwebapi --resource-group marumacosmosdbwebapi

- az cosmosdb database create --db-name root --url-connection COSMOS_URI --key COSMOS_KEY

- az cosmosdb collection create --collection-name orders --db-name root --partition-key-path "/id" --throughput 400 --url-connection COSMOS_URI --key COSMOS_KEY

**Open up Azure**

Display the newly created CosmosDB database.

**Open CosmosDBWebApi project and update configuration**

```
"Azure": {
    "CosmosDB": {
        "DatabaseId": "root",
        "Key": "COSMOS_KEY",
        "Endpoint": "COSMOS_URI"
    }
}
``` 

**Open up Postman**

I am using the Postman tests to save the id variables of the entities I am created so they can automatically be used in other calls.

## Review Code ##

I think we are ready now to review the code, if you have any questions please feel free to jump in and I will do my best to answer them.

If for some reason I can't answer them now, I will take getting an answer for you as an action item.

Our example is based on an Orders entity composed of shopper information, which also contains a collection of Order Items, things our shopper has purchased. 

```
{
    "id": "f86938c8-590e-4231-bec3-7065d0ebbfcc",
    "shopperName": "Malcolm Reynolds",
    "shopperEmail": "mal@serenity.com",
    "items": [
    {
        "id": "089f4466-a586-4fa3-91dc-b1d431ec6918",
        "name": "Widget 1",
        "quantity": 1,
        "isTaxable": true
    },
    {
        "id": "48195071-6432-436e-963b-b86b87492882",
        "name": "Widget 2",
        "quantity": 1,
        "isTaxable": false
    }]
}
```
Followed the repository pattern.

Repositories are classes or components that encapsulate the logic required to access data sources.

- They centralize common data access functionality,
- providing better maintainability
- and decoupling the infrastructure or technology used to access databases from the domain model layer.

https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design

### Cosmos 2.0 SDK ###

*CosmosDbSdk2Repository* - What the repositories inherit from, creates the initial connection.

*IOrderRepository* - Base repository for Orders, supports standard CRUD methods.

*IOrderCosmosDbSdk2Repository*, *IOrderCosmosDbSdk2Repository* - Separate implementations based on the SDKs, just did this support the standard DI implementation.

*OrderCosmosDbSdk2Repository* - You will notice that we are injecting the settings for the CosmosDB, look at *IOptions&lt;AzureCosmosDbOptions&gt; azureCosmosDbOptions*.

This is a nice way to inject configuration especially if you want to use things Azure Key Vault, pull from the vault in the production, pull from `appsettings.json` in development.

- It's pretty verbose.
- There is a dependency on dynamic objects.
- Does offer some LINQ support, but not as comprehensive like if you were working with Azure SQL.

*OrdersController*

### Cosmos 3.0 SDK ###

*CosmosDbSdk3Repository*

*OrderCosmosDbSdk3Repository*

- Not as verbose.
- Better class naming, primary classes start with Cosmos*.
- More of a fluent approach.
- All types are not mockable, which makes for better testing.

*OrdersController*

It is exactly like the v1 *OrdersController*, except it injects the repository that supports the Cosmos 3.0 SDK.

Quick look at how to work with items within a collection in a document, in our case Items, or Order Items for an Order.

*OrderItemsController*

*OrderItemCosmosDbSdk3Repository*

## Conclusion ## 

That's all I have for now, if there is some area you would like to take a deeper dive into, please let me know and we can have a conversation.

Any questions specific to this demo?

If you would like access to the GitHub repository please email me at [maruma@microsoft.com](http://mailto:maruma@microsoft.com).