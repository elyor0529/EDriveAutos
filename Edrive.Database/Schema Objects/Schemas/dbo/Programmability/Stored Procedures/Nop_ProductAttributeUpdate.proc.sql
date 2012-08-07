

CREATE PROCEDURE [dbo].[Nop_ProductAttributeUpdate]
(
	@ProductAttributeID int,
	@Name nvarchar(100),
	@Description nvarchar(4000)
)
AS
BEGIN

	UPDATE [Nop_ProductAttribute]
	SET
		[Name]=@Name,
		[Description]=@Description
	WHERE
		ProductAttributeID = @ProductAttributeID
END
