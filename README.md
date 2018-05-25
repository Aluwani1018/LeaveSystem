# LeaveSystem

## Business Case
#### Employee
As an Employee I want to Capture Leave
* Annual Leave 
I want the ability to view my leave requests over time. I also want the ability to edit and retract pending leave requests.
Manager
As a manager, I want to approve or reject leave that my subordinates requested. I want to be able to see all leave my subordinates requested over time. 
Business Requirements
*	Work days are Monday to Friday (excluding public holidays).


## Getting Started

Make sure you have visual studio Installed in your machine preferable (2017 version) with all the MSSQL tools installed.
Clone the project and Build.
* Add migration 
```
Add-Migration Init
```
```
Update-Database
```


#### Signin details

Admin
```
User Name: admin@company1.com
Password: Password.1
```

## Built With

* Asp.Net Core 2.0
* MVC 6

## Patterns Used

* Repository Pattern
