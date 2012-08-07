

CREATE PROCEDURE [dbo].[Nop_ProductTagInsert]
(
	@ProductTagID int = NULL output,
	@Name nvarchar(100),
	@ProductCount int
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductTag]
	(
		[Name],
		[ProductCount]
	)
	VALUES
	(
		@Name,
		@ProductCount
	)

	set @ProductTagID=SCOPE_IDENTITY()
END
