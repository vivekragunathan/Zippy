## Zippy

_Zippy is the pet name of the pre-assignment project_. 😜

>To know more about the problem, read [here](README.md).

**NOTES**

- Implemented as ASP.NET Core MVC project
	- Implemented two service methods
		- **`/locate`**: Locate Person(s)
			- POST API that takes one or more `LocateRequest` () objects and returns the list of corresponding `Person` objects (Name, Address, Zip)
			- A convenient GET API that does the same job as the above POST API but only for a single person
		- **`/persons?zip={zipcode}`**:  GET API that takes a zipcode in the query parameter as input and returns a list of `Person`s for the specified zipcode; empty if no persons exist for the specified zip
		- **`/person/{name}`**: Convenience GET API to find a person by name. This API does not register the person instead finds the person already registered.
	- Service methods are ASP.NET Web API based and return JSON data. User interface hasn't been developed (lack of time!). The service/APIs can be tested using any REST client such as [POSTMAN](http://www.getpostman.com).
	- Namespaces and classes have been named as per the component or facility they would represent in realtime.
	- `ZResponse` is the standard response for all APIs. It consists of `status`, `message, and `payload`. The `payload` depends on the API and the scenario. Error handling has been implemented from a moderate to a sufficient extent to let the client know of the error. For instance, even when handling the `locate` POST request, error handling has been implemented to the individual name/address level.
	- The initial idea was to use one of the two options for data persistence
		- Simple - LiteDB: simple fast NoSQL like serveless single-file-based database for virtually all kinds of environments (mobile ready)
		- Ideal - MongoDB or RavenDB

		First choice was LiteDB so that it is easy to demonstrate the data persistence functionality in the application. There were issues loading saved data, which was taking time to resolve. To save time and produce a working solution that demonstrates data persistence, a third dumber (rather than simpler) approach - JSON file dump, has been adopted. For that matter, even the respective class has been named `DumpDbDriver`. Although it cannot be an option for real time, `DumpDbDriver` demonstrates the data persistence abilities in the context of Zipper.
- Project/Code Organization
	- `Controllers/`
	- `Services/`
	- `Models/`
	- `Data/`
	- `Utils/`
- **RUN**
	- Ensure [.NET Core](https://www.microsoft.com/net/download/core) is installed on your machine. Works for Linux, Mac and Windows~
	- `git clone git@gitlab.com:VivekRagunathan/Zippy.git`
	- In the source folder (`{wherever downloaded path}/Zippy`), `dotnet restore`, `dotnet build`, `dotnet run`
	- App should start on `http://localhost:5000`
- **TEST**
	- If not installed already, install [POSTMAN](http://www.getpostman.com).
	- Import [this](ZIPPY.postman_collection.json) preloaded API collection in POSTMAN.
	- Invoke the APIs in the collection organized by API-wise named folders (`FindPerson`, `FindPersonByZip` etc.)