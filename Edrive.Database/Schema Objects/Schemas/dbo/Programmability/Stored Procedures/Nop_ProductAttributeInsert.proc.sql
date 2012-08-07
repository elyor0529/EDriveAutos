

CREATE PROCEDURE [dbo].[Nop_ProductAttributeInsert]
(
	@ProductAttributeID int = NULL output,
	@Name nvarchar(100),
	@Description nvarchar(4000)
)
AS
BEGIN
	INSERT
	INTO [Nop_ProductAttribute]
	(
		[Name],
		[Description]
	)
	VALUES
	(
		@Name,
		@Description
	)

	set @ProductAttributeID=SCOPE_IDENTITY()
END
