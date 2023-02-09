# EmpPayrollBackend
Backend for employ payroll using asp.net core web api, database first approach of Entity frame work

Code in postgreSQL

CREATE TABLE Users(
	Id serial PRIMARY KEY,
	Fullname varchar (200),
	Email varchar (100),
	Mobile varchar(30),
	Password varchar(150)
	);
	
CREATE TABLE EmployDetail(
	EmployeeId serial PRIMARY KEY,
	Name varchar (200),
	Profile varchar (100),
	Gender varchar(30),
	Department varchar(150),
	Salary varchar,
	StartDate varchar
	);
	
	Create or replace procedure SP_Register
(
	Fullname varchar(200),
	Email varchar(200),
	Mobile varchar(100),
	Password varchar(100)
)
Language sql
as
$$
		Insert into Users (Fullname,Email,Mobile,Password)    
		Values (Fullname,Email,Mobile,Password) 
 
$$

Insert into Users(Fullname,Email,Mobile,Password) Values('PabbojiShravanthi','shravanthi27041996@gmail.com',1234567890,'Shravanthi@123');

select * from Users;

delete from Users where id=3;
-----Login----------
create or replace procedure SP_Login
(
	Email varchar(200),
	Password varchar(100)
)
Language sql
as
$$
select * from Users where Email=Email and Password=Password
$$
----SP-Delete-------------------------
create or replace PROCEDURE Sp_Delete
	(EmployeeId integer)
Language sql
as
$$
delete from EmployDetail where EmployeeId=EmployeeId;

	SELECT * from EmployDetail;
$$
-------Get All Employees------------
Create or replace procedure spGetAllEmployees()      
Language sql
as
$$      
    select * from EmployDetail;    
$$
-----Get Single Employee------
Create or replace procedure spGetEmploy
(EmployId integer)
Language sql
as
$$
    select * from EmployDetail where EmployeeId=EmployId;    
$$

------Add Employ
Create or replace PROCEDURE Sp_AddEmployee
(Name varchar(200),
Profile varchar(200),Gender varchar(200),Department varchar(200),Salary varchar,
StartDate varchar)
language sql
AS
$$
insert into EmployDetail(Name,Profile,Gender,Department,Salary,StartDate)
values(Name,Profile,Gender,Department,Salary,StartDate);
SELECT * from EmployDetail;
$$

ALTER TABLE EmployDetail
ALTER COLUMN StartDate TYPE varchar;

ALTER TABLE EmployDetail
ALTER COLUMN salary TYPE varchar;
