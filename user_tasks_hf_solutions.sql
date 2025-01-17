CREATE DATABASE UserTasks;
GO

USE UserTasks;
GO

CREATE LOGIN hfsolutions WITH PASSWORD='hfsolutions', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF;
GO

USE UserTasks;
GO

CREATE USER hfsolutions FOR LOGIN hfsolutions;
GO

USE UserTasks;
GO

ALTER ROLE db_owner ADD MEMBER hfsolutions;
GO

CREATE TABLE UserTasks.dbo.TaskState (
	TaskStateId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_TaskState PRIMARY KEY (TaskStatusId)
);

CREATE TABLE UserTasks.dbo.[User] (
	UserId int IDENTITY(1,1) NOT NULL,
	UserName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_User PRIMARY KEY (UserId)
);

CREATE TABLE UserTasks.dbo.UserTask (
	UserTaskId int IDENTITY(1,1) NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ExpirationDate datetime2 NOT NULL,
	TaskStateId int NOT NULL,
	UserId int NOT NULL,
	CONSTRAINT PK_UserTask PRIMARY KEY (UserTaskId)
);

ALTER TABLE UserTasks.dbo.UserTask ADD CONSTRAINT FK_UserTask_TaskState_TaskStateId FOREIGN KEY (TaskStateId) REFERENCES UserTasks.dbo.TaskState(TaskStatusId) ON DELETE NO ACTION;
ALTER TABLE UserTasks.dbo.UserTask ADD CONSTRAINT FK_UserTask_User_UserId FOREIGN KEY (UserId) REFERENCES UserTasks.dbo.[User](UserId) ON DELETE CASCADE;

INSERT INTO UserTasks.dbo.TaskState (Name)
VALUES
('Pendiente'),
('En progreso'),
('Completada');
GO