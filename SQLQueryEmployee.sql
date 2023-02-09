CREATE TABLE EmployDetails(
	EmployeeId int Identity(1,1) PRIMARY KEY,
	Name varchar (200),
	Profile varchar (100),
	Gender varchar(30),
	Department varchar(150),
	Salary int,
	StartDate Date
	);

	--Sp --- Add Employee ---------------
go
Create or ALTER PROCEDURE Sp_AddEmployee
@Name varchar(200),
@Profile varchar(200),@Gender varchar(200),@Department varchar(200),@Salary varchar(200),
@StartDate date

AS
BEGIN
insert into EmployDetails(Name,Profile,Gender,Department,Salary,StartDate)
values(@Name,@Profile,@Gender,@Department,@Salary,@StartDate);
SELECT * from EmployDetails
END

--Sp --- Delete Employee ---------------
go
create or ALTER PROCEDURE [dbo].[Sp_Delete]
	@EmployeeId int
AS
BEGIN
delete from EmployDetails where EmployeeId=@EmployeeId

	SELECT * from EmployDetails
END

--Sp --- Update ---------------------
Go
Create procedure spUpdateEmployee          
(  
@EmpId int,
@Name varchar(200),
@Profile varchar(200),@Gender varchar(200),@Department varchar(200),@Salary varchar(200),
@StartDate date        
)          
as          
begin          
   Update EmployDetails           
   set Name=@Name, 
   profile=@Profile, 
   Gender=@Gender,
   Department=@Department,        
   Salary=@Salary,
   StartDate=@StartDate
   where EmployeeId=@EmpId          
End    

--Sp -- Get All Employees -----
Go
Create or alter procedure spGetAllEmployees      
as      
Begin      
    select * from EmployDetails      
End

---Sp---Get Employ by ID------------

Go
Create or alter procedure spGetEmploy
@EmployId int
as      
Begin      
    select * from EmployDetails where EmployeeId=@EmployId      
End
