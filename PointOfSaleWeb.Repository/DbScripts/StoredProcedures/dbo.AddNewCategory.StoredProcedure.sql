USE [Inventory]
GO
	/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 4/6/2023 10:17:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO ALTER PROCEDURE [dbo].[AddNewCategory] @CategoryName varchar(50),
	@CategoryID INT = 0 AS BEGIN
SET NOCOUNT ON;
IF EXISTS (
	SELECT 1
	FROM [dbo].[Categories]
	WHERE [CategoryName] = @CategoryName
) BEGIN RAISERROR (
	'Category %s already exists!',
	16,
	1,
	@CategoryName
);
RETURN;
END BEGIN TRY;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Categories] (CategoryName)
VALUES (@CategoryName);
COMMIT TRANSACTION;
SELECT @CategoryID = SCOPE_IDENTITY();
EXEC GetCategoryById @CategoryID = @CategoryID;
END TRY BEGIN CATCH;
IF (@@TRANCOUNT > 0) BEGIN;
ROLLBACK TRANSACTION;
END;
PRINT 'Error ocurred in ' + ERROR_PROCEDURE() + ' ' + ERROR_MESSAGE();
RETURN -1;
END CATCH;
RETURN 0;
END;