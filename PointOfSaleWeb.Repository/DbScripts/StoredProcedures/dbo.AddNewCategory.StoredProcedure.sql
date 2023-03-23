USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 3/22/2023 8:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewCategory]
    @CategoryName varchar(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Categories WHERE CategoryName = @CategoryName)
        BEGIN
            THROW 51000, 'Category Name already exists!', 1;
        END

        INSERT INTO Categories (CategoryName)
        VALUES (@CategoryName);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END
GO
