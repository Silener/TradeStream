IF NOT EXISTS( SELECT * FROM SYS.databases WHERE NAME = '<DatabaseName, sysname, Database name>' )
BEGIN

	CREATE DATABASE <DatabaseName, sysname, Database name>
    ON (NAME = '<DatabaseName, sysname, Database name>', FILENAME = '<DatabasePath, sysname, Path of the database. Ex: D:\Databases\><DatabaseName, sysname, Database name>.mdf', SIZE = 1024MB, FILEGROWTH = 256MB)
	LOG ON (NAME = '<DatabaseLogName, sysname, Database log name>', FILENAME = '<DatabaseLogPath, sysname, Path of the database log. Ex: D:\Databases\><DatabaseLogName, sysname, Database log name>.ldf', SIZE = 512MB, FILEGROWTH = 125MB)

END
GO

USE <DatabaseName, sysname, Database name>;
GO

IF EXISTS( SELECT * FROM SYS.TABLES WITH(NOLOCK) WHERE NAME = 'DATA_STREAMS' )
	DROP TABLE DATA_STREAMS
GO

CREATE TABLE DATA_STREAMS
(
	ID INT IDENTITY(1, 1) NOT NULL,
	RECORD_DATE DATETIME NOT NULL,
	SYMBOL_TYPE SMALLINT NOT NULL,
	PRICE FLOAT NOT NULL,
	CONSTRAINT PK_DATA_STREAMS_ID PRIMARY KEY(ID)
)
GO

CREATE NONCLUSTERED INDEX IX_DATA_STREAMS_SYMBOL_TYPE_RECORD_DATE
ON DATA_STREAMS( SYMBOL_TYPE, RECORD_DATE )
GO