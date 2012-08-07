

CREATE PROCEDURE [dbo].[Nop_CustomerRole_Discount_MappingInsert]
(
	@CustomerRoleID int,
	@DiscountID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_CustomerRole_Discount_Mapping] WHERE CustomerRoleID=@CustomerRoleID and DiscountID=@DiscountID)
	INSERT
		INTO [Nop_CustomerRole_Discount_Mapping]
		(
			CustomerRoleID,
			DiscountID
		)
		VALUES
		(
			@CustomerRoleID,
			@DiscountID
		)
END
