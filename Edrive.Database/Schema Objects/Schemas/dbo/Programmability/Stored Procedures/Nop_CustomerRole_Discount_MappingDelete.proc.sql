

CREATE PROCEDURE [dbo].[Nop_CustomerRole_Discount_MappingDelete]
(
	@CustomerRoleID int,
	@DiscountID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerRole_Discount_Mapping]
	WHERE
		CustomerRoleID = @CustomerRoleID and DiscountID=@DiscountID
END
