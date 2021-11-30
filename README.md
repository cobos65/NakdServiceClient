# NakdServiceClient
Description:
  Implement a Client for Nakd customer RestApi
  
Approach and comments:

1- This client has a 3 layer architechture trying to divide the UI (Console), Busines, and data Retrieval (Services)

2- Interfaces are splited in projects to enable distribute it in nugets or dlls separately from the implementation.

3- Use .net core because is a technology i want work on, and because offers a lot of advantage between la classic framework. Operative system for example.

4- Tests are developed using MS tests, Moq as the mocking tool and Shouldly as an assertion tool due to is what i'm using now, and its faster for me.

Improvements.

1- Improve code coeverage

2- Add more validations before send the call to the API

3- Create a Services project to hold HTTPRestHelper to be easier to reuse it using nugets.

4- In the case that the source of the data could be other than a REST API we can create another layer like IDataProvider

  3.1- The classes that inherit from IDataProvider should implement the contract and it can do the data retrieval as it needs.

  3.1.1- For example we can have a IDatabaseProvider or a IRestApiProvider.
 
 Time investment: ~6 hours
