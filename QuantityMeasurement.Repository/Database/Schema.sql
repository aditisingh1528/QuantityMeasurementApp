-- ============================================================
-- QuantityMeasurementDB - SQL Server Schema
-- UC16: Database Integration for Quantity Measurement App
-- ============================================================

IF DB_ID('QuantityMeasurementDB') IS NULL
BEGIN
    CREATE DATABASE QuantityMeasurementDB;
    PRINT 'Database QuantityMeasurementDB created.';
END
ELSE
BEGIN
    PRINT 'Database QuantityMeasurementDB already exists. Skipping creation.';
END
GO

USE QuantityMeasurementDB;
GO


-- Drop trigger first (it depends on the main table)
IF OBJECT_ID('TRG_InsertMeasurementHistory', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER TRG_InsertMeasurementHistory;
    PRINT 'Trigger TRG_InsertMeasurementHistory dropped.';
END
GO

-- Drop history table
IF OBJECT_ID('QuantityMeasurementHistory', 'U') IS NOT NULL
BEGIN
    DROP TABLE QuantityMeasurementHistory;
    PRINT 'Table QuantityMeasurementHistory dropped.';
END
GO

-- Drop main table
IF OBJECT_ID('QuantityMeasurements', 'U') IS NOT NULL
BEGIN
    DROP TABLE QuantityMeasurements;
    PRINT 'Table QuantityMeasurements dropped.';
END
GO

CREATE TABLE QuantityMeasurements
(
    -- Auto-incrementing primary key
    Id                  INT           IDENTITY(1,1) PRIMARY KEY,

    OperationType       NVARCHAR(50)  NOT NULL,

    MeasurementCategory NVARCHAR(50)  NOT NULL,

    -- First operand
    Operand1Value       FLOAT         NOT NULL,
    Operand1Unit        NVARCHAR(50)  NOT NULL,

    -- Second operand
    Operand2Value       FLOAT         NULL,
    Operand2Unit        NVARCHAR(50)  NULL,

    -- Result
    ResultValue         FLOAT         NULL,
    ResultUnit          NVARCHAR(50)  NULL,

    -- Error message
    ErrorMessage        NVARCHAR(500) NULL,

    Timestamp           DATETIME      DEFAULT GETDATE()
);
GO

PRINT 'Table QuantityMeasurements created.';
GO

CREATE INDEX IDX_OperationType
    ON QuantityMeasurements(OperationType);

CREATE INDEX IDX_MeasurementCategory
    ON QuantityMeasurements(MeasurementCategory);

CREATE INDEX IDX_Timestamp
    ON QuantityMeasurements(Timestamp);
GO

PRINT 'Indexes created on QuantityMeasurements.';
GO

CREATE TABLE QuantityMeasurementHistory
(
    HistoryId           INT           IDENTITY(1,1) PRIMARY KEY,

    MeasurementId       INT,

    OperationType       NVARCHAR(50),
    MeasurementCategory NVARCHAR(50),
    Operand1Value       FLOAT,
    Operand1Unit        NVARCHAR(50),
    Operand2Value       FLOAT         NULL,
    Operand2Unit        NVARCHAR(50)  NULL,
    ResultValue         FLOAT         NULL,
    ResultUnit          NVARCHAR(50)  NULL,
    ErrorMessage        NVARCHAR(500) NULL,
    Timestamp           DATETIME,

    AuditAction         NVARCHAR(50),

    AuditTimestamp      DATETIME      DEFAULT GETDATE()
);
GO

PRINT 'Table QuantityMeasurementHistory created.';
GO

CREATE TRIGGER TRG_InsertMeasurementHistory
ON QuantityMeasurements
AFTER INSERT
AS
BEGIN
    INSERT INTO QuantityMeasurementHistory
    (
        MeasurementId,
        OperationType,
        MeasurementCategory,
        Operand1Value,
        Operand1Unit,
        Operand2Value,
        Operand2Unit,
        ResultValue,
        ResultUnit,
        ErrorMessage,
        Timestamp,
        AuditAction
    )
    SELECT
        Id,
        OperationType,
        MeasurementCategory,
        Operand1Value,
        Operand1Unit,
        Operand2Value,
        Operand2Unit,
        ResultValue,
        ResultUnit,
        ErrorMessage,
        Timestamp,
        'INSERT'
    FROM inserted;
END
GO

PRINT 'Trigger TRG_InsertMeasurementHistory created.';
GO

-- Save a new measurement record
CREATE PROCEDURE sp_SaveMeasurement
(
    @OperationType       NVARCHAR(50),
    @MeasurementCategory NVARCHAR(50),
    @Operand1Value       FLOAT,
    @Operand1Unit        NVARCHAR(50),
    @Operand2Value       FLOAT         = NULL,
    @Operand2Unit        NVARCHAR(50)  = NULL,
    @ResultValue         FLOAT         = NULL,
    @ResultUnit          NVARCHAR(50)  = NULL,
    @ErrorMessage        NVARCHAR(500) = NULL
)
AS
BEGIN
    INSERT INTO QuantityMeasurements
    (
        OperationType, MeasurementCategory,
        Operand1Value, Operand1Unit,
        Operand2Value, Operand2Unit,
        ResultValue,   ResultUnit,
        ErrorMessage
    )
    VALUES
    (
        @OperationType, @MeasurementCategory,
        @Operand1Value, @Operand1Unit,
        @Operand2Value, @Operand2Unit,
        @ResultValue,   @ResultUnit,
        @ErrorMessage
    );
END
GO

-- Get all measurements ordered by newest first
CREATE PROCEDURE sp_GetAllMeasurements
AS
BEGIN
    SELECT *
    FROM QuantityMeasurements
    ORDER BY Timestamp DESC;
END
GO

-- Get measurements filtered by operation type
CREATE PROCEDURE sp_GetMeasurementsByOperation
    @OperationType NVARCHAR(50)
AS
BEGIN
    SELECT *
    FROM QuantityMeasurements
    WHERE OperationType = @OperationType
    ORDER BY Timestamp DESC;
END
GO

-- Get measurements filtered by category
CREATE PROCEDURE sp_GetMeasurementsByCategory
    @MeasurementCategory NVARCHAR(50)
AS
BEGIN
    SELECT *
    FROM QuantityMeasurements
    WHERE MeasurementCategory = @MeasurementCategory
    ORDER BY Timestamp DESC;
END
GO

-- Get total count of all measurements
CREATE PROCEDURE sp_GetTotalMeasurements
AS
BEGIN
    SELECT COUNT(*) AS TotalMeasurements
    FROM QuantityMeasurements;
END
GO

-- Delete all measurements
CREATE PROCEDURE sp_DeleteAllMeasurements
AS
BEGIN
    DELETE FROM QuantityMeasurements;
END
GO

-- Get full audit history
CREATE PROCEDURE sp_GetMeasurementHistory
AS
BEGIN
    SELECT *
    FROM QuantityMeasurementHistory
    ORDER BY AuditTimestamp DESC;
END
GO

PRINT 'All stored procedures created.';
GO

SELECT
    'Table' AS ObjectType,
    name    AS ObjectName
FROM sys.tables
WHERE name IN ('QuantityMeasurements', 'QuantityMeasurementHistory')

UNION ALL

SELECT
    'Index' AS ObjectType,
    name    AS ObjectName
FROM sys.indexes
WHERE name IN ('IDX_OperationType', 'IDX_MeasurementCategory', 'IDX_Timestamp')

UNION ALL

SELECT
    'Trigger'  AS ObjectType,
    name       AS ObjectName
FROM sys.triggers
WHERE name = 'TRG_InsertMeasurementHistory'

UNION ALL

SELECT
    'StoredProcedure' AS ObjectType,
    name              AS ObjectName
FROM sys.procedures
WHERE name IN (
    'sp_SaveMeasurement',
    'sp_GetAllMeasurements',
    'sp_GetMeasurementsByOperation',
    'sp_GetMeasurementsByCategory',
    'sp_GetTotalMeasurements',
    'sp_DeleteAllMeasurements',
    'sp_GetMeasurementHistory'
)
ORDER BY ObjectType, ObjectName;
GO
