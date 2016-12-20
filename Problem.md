# Pre Assignment/Craft Demo

## Problem Statement

Implement a service that fetches and stores “User Profile” information.

- **POST API**: Given a list of 5 street addresses (as below), obtain the postal code using [Google Maps Geocoding](https://developers.google.com/maps/documentation/geocoding/start) API. The API should return the user names along with complete addresses of the users (including postal code).
	- Name: Dominick Desoto
	- Address: 2700 Coast Ave, Mountain View CA

	- Name: Lakendra Mcandrew
	- Address: 5601 Headquarters Drive, Plano Texas
	
	- Name: Scottie Abraham
	- Address: 7535 Torrey Santa Fe Road San Diego, CA
	
	- Name: Alphonse Heiss
	- Address: 21215 Burbank Boulevard Woodland Hills, CA
	
	- Name: Chan Pullman
	- Address: 2800 E. Commerce Center Place Tucson, AZ

- **GET API**: Given a postal code, return all the users in that postal code (The API should return user name along with complete address for that postal code)
 
- **Extra Credit**: Make the GET API return results even after the service is shut down and restarted again and no POST API call was made.


Upload your code to your GitHub and share the link with us 24 hours before the interview. Be prepared to run your demo during the interview on your laptop if possible.