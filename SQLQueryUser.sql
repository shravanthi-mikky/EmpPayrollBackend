create database EmpAngularjs;

CREATE TABLE Users(
	Id int Identity(1,1) PRIMARY KEY,
	Fullname varchar (200),
	Email varchar (100),
	Mobile varchar(30),
	Password varchar(150)
	);

GO
Create or ALTER procedure SP_Register
(
	@Fullname varchar(200),
	@Email varchar(200),
	@Mobile varchar(100),
	@Password varchar(100)
)
as
begin
IF (select Id from Users where Email=@Email) is not null 
	begin
		select 1;
	end
	else
	begin   
		Insert into Users (Fullname,Email,Mobile,Password)    
		Values (@Fullname,@Email,@Mobile,@Password) 
	end   
END

---------SP for Login-----------------
Go
create or ALTER procedure SP_Login
(
	@Email varchar(200),
	@Password varchar(100)
)
as
begin
select * from Users where Email=@Email and Password=@Password

END

-----------------------------------Forget password-stored procedure-------------
Go
create or ALTER PROCEDURE [dbo].[SpForgetPass]
@Email varchar(200)
AS
BEGIN
	SELECT * from Users where Email=@Email
END
--------------------------------Reset Password-------------------------------------------------
Go
create or ALTER PROCEDURE [dbo].[SpReset]
@Email varchar(200),
@Password varchar(200)
AS
BEGIN
	Update Users Set Password=@Password where Email=@Email
END

select * from Users;