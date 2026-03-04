# Setup / configuration
1. Create the database and tables

	```Scripts/CreateIdentityTables.sql```
  
	- This will create Identity Framework tables under 'auth' schema).
	
  *(For now the schema is a hardcoded string in TableBase.cs. This might be configurable in a future release)*

2. Add NuGet package to project
	    
		dotnet add package avans.identity.dapper
		
3. Configure Identity Framework middleware to use this package for persistance

	Add 'AddDapperStores' instead of 'UseEntityFramework':
	```csharp
	use Avans.Identity.Dapper
	
	// ...
	
	// Identity configuration -->
	builder.Services
	// API Endpoints (and optionally configure options) -->
	.AddIdentityApiEndpoints<IdentityUser>(options => {
		})
	.AddRoles<IdentityRole>()
	// Tell IF to use the Dapper stores from Avans.Identity.Dapper -->
	.AddDapperStores(options => {
		options.ConnectionString = dbConnectionString;
	});
	```	
