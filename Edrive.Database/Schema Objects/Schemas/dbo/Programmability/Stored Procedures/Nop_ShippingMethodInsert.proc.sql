

CREATE PROCEDURE [dbo].[Nop_ShippingMethodInsert]
(
	@ShippingMethodID int = NULL output,
	@Name nvarchar(100),
	@Description nvarchar(2000),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_ShippingMethod]
	(
		[Name],
		[Description],
		DisplayOrder
	)
	VALUES
	(
		@Name,
		@Description,
		@DisplayOrder
	)

	set @ShippingMethodID=SCOPE_IDENTITY()
END
