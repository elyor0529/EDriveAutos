

CREATE PROCEDURE [dbo].[Nop_ShippingMethodUpdate]
(
	@ShippingMethodID int,
	@Name nvarchar(100),
	@Description nvarchar(2000),
	@DisplayOrder int
)
AS
BEGIN
	UPDATE [Nop_ShippingMethod]
	SET
		[Name]=@Name,
		[Description]=@Description,
		DisplayOrder=@DisplayOrder
	WHERE
		ShippingMethodID = @ShippingMethodID
END
