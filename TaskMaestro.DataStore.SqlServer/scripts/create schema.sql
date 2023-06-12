DROP INDEX IF EXISTS Tasks_CreatedAt ON maestro.Tasks;

DROP TABLE IF EXISTS maestro.Tasks;
DROP TABLE IF EXISTS maestro.Acks;
DROP TABLE IF EXISTS maestro.TaskAcks;

DROP TYPE IF EXISTS maestro.TaskType;
DROP TYPE IF EXISTS maestro.AckType;
DROP TYPE IF EXISTS maestro.TaskAckType;

DROP SCHEMA IF EXISTS maestro;

CREATE SCHEMA maestro;

CREATE TABLE maestro.Tasks(
	Id BINARY(16) NOT NULL PRIMARY KEY,
	[Type] TINYINT NOT NULL,
	[Input] VARBINARY(MAX) NOT NULL,
	InputType NVARCHAR(500) NOT NULL,
	AckCode BINARY(20) NOT NULL,
	AckValueType NVARCHAR(500) NOT NULL,
	GroupId BINARY(16) NULL,
	HandlerType NVARCHAR(500) NOT NULL,
	Queue NVARCHAR(100) NOT NULL,
	CreatedAt DATETIME2 NOT NULL,
	FetchedAt DATETIME2 NULL,
	CompletedAt DATETIME2 NULL,
	Status TINYINT NOT NULL,
	MaxRetryCount INT NOT NULL,
	CurrentRetryCount INT NOT NULL
);

CREATE INDEX Tasks_CreatedAt ON maestro.Tasks(Queue, CreatedAt) WHERE FetchedAt IS NULL;

CREATE TABLE maestro.TaskExecutionReports(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	TaskId BINARY(16) NOT NULL,
	[Type] TINYINT NOT NULL,
	Message VARCHAR(MAX) NULL,
	CreatedAt DATETIME2 NOT NULL
);

CREATE INDEX TaskExecutionReports_TaskId ON maestro.TaskExecutionReports(TaskId);

CREATE TYPE maestro.TaskType AS TABLE(
	Id BINARY(16) NOT NULL PRIMARY KEY,
	[Type] TINYINT NOT NULL,
	[Input] VARBINARY(MAX) NOT NULL,
	InputType NVARCHAR(500) NOT NULL,
	AckCode BINARY(20) NOT NULL,
	AckValueType NVARCHAR(500) NOT NULL,
	GroupId BINARY(16) NULL,
	HandlerType NVARCHAR(500) NOT NULL,
	Queue NVARCHAR(100) NOT NULL,
	CreatedAt DATETIME2 NOT NULL,
	FetchedAt DATETIME2 NULL,
	CompletedAt DATETIME2 NULL,
	Status TINYINT NOT NULL,
	MaxRetryCount INT NOT NULL,
	CurrentRetryCount INT NOT NULL
);

declare @myTable as maestro.TaskType
declare @test as table( Id as int, name as varchar(12) )

CREATE TABLE maestro.Acks(
	Code BINARY(20) NOT NULL PRIMARY KEY,
	[Value] VARBINARY(MAX) NOT NULL,
	ValueType NVARCHAR(500) NOT NULL,
	CreatedAt DATETIME2 NOT NULL
);

CREATE TYPE maestro.AckType AS TABLE(
	Code BINARY(20) NOT NULL PRIMARY KEY,
	[Value] VARBINARY(MAX) NOT NULL,
	ValueType NVARCHAR(500) NOT NULL,
	CreatedAt DATETIME2 NOT NULL
);

CREATE TABLE maestro.TaskAcks(
	TaskId BINARY(16) NOT NULL,
	Code BINARY(20) NOT NULL,
	PRIMARY KEY(TaskId, Code)
);

CREATE TYPE maestro.TaskAckType AS TABLE(
	TaskId BINARY(16) NOT NULL,
	Code BINARY(20) NOT NULL,
	PRIMARY KEY(TaskId, Code)
);
