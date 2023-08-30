Create Database TeamDb
use TeamDb
create Table Players 
(
PlayerId int Primary key,
FirstName nvarchar(50),
LastName nvarchar(50),
JerseyNumber int,
Position int, 
Team nvarchar(50),
)
insert into Players values (1,'Sachin','Tendulkar',100,1,'India');
insert into Players values (2,'Dhoni','Mahendra',07,2,'India');
Select * from Players