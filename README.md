## NETSecurity
### A project to address Security concerns for .NET

A Demo application that demostrates how to work with MVC, Web API and .NET Security 
The project tries to represent a classic CQRS architecture (Command, Query Responsability Segregation) explained by M. Fowler here : http://martinfowler.com/bliki/CQRS.html and address all the available scenarios related to Security.

The project has the following structure:
- User Interface
 - A Client for Web, using ASP.NET MVC and Knockout.JS
 - A Client for Windows using WPF (Windows Presentation Foundation)
- Core
 - A Read Domain API using Web API 2.2
 - A Write Domain API using Web API 2.2
 - A Domain Model using the principles of SOLID and DDD
 - A Data Layer using Entity Framework Code First 6.1.1
 - A Security Service using ASP.NET Identity to cover all the aspects of Security
- Tests
 - Each project is covered with a Unit/Integration test
 
 ### Concerns addressed by this Project:
- **Authentication**
 - Anonymous authentication
 - Social Claims authentication (Google+, Facebook and Twitter)
 - Active Directory authentication
 - User storage and configuration
 
- **Authorization**
 - Web API controller Authorization
 - MVC Web Page Authorization
 - Web API controller method Authorization
 - Roles and Actions permissions






