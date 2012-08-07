

CREATE PROCEDURE [dbo].[Nop_Customer_CustomerRole_MappingInsert]
(
	@CustomerID int,
	@CustomerRoleID int
)
AS
BEGIN
	IF NOT EXISTS (SELECT (1) FROM [Nop_Customer_CustomerRole_Mapping] WHERE CustomerID=@CustomerID and CustomerRoleID=@CustomerRoleID)
	INSERT
		INTO [Nop_Customer_CustomerRole_Mapping]
		(
			CustomerID,
			CustomerRoleID
		)
		VALUES
		(
			@CustomerID,
			@CustomerRoleID
		)
END
