USE [POS]
GO
	/****** Object:  Table [dbo].[Categories]    Script Date: 4/8/2023 5:21:54 PM ******/
SET
	ANSI_NULLS ON
GO
SET
	QUOTED_IDENTIFIER ON
GO
	CREATE TABLE [dbo].[Categories](
		[CategoryID] [int] IDENTITY(1, 1) NOT NULL,
		[CategoryName] [nvarchar](50) NOT NULL,
		CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC) WITH (
			PAD_INDEX = OFF,
			STATISTICS_NORECOMPUTE = OFF,
			IGNORE_DUP_KEY = OFF,
			ALLOW_ROW_LOCKS = ON,
			ALLOW_PAGE_LOCKS = ON,
			OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

CREATE TRIGGER PreventDeleteUpdateWithoutWhereOnCategories
ON Categories
FOR DELETE, UPDATE
AS
BEGIN
  IF NOT EXISTS (SELECT 1 FROM deleted)
  BEGIN
    RAISERROR('Delete or update operation without a WHERE clause is not allowed in the Categories table.', 16, 1)
    ROLLBACK TRANSACTION
  END
END;
GO