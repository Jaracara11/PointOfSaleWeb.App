USE [POS]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 3/22/2023 8:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCategory]
    @CategoryID int
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryID = @CategoryID)
        BEGIN
            THROW 51000, 'Category not found!', 1;
        END

        DELETE FROM Categories WHERE CategoryID = @CategoryID;

        -- Reset identity seed of CategoryID column
        DECLARE @MaxCategoryID INT;
        SELECT @MaxCategoryID = ISNULL(MAX(CategoryID),0) FROM Categories;
        DBCC CHECKIDENT('Categories', RESEED, @MaxCategoryID);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END
GO
