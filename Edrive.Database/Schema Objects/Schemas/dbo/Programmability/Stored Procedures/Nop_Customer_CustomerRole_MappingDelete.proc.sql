

CREATE PROCEDURE [dbo].[Nop_Customer_CustomerRole_MappingDelete]
(
	@CustomerID int,
	@CustomerRoleID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Customer_CustomerRole_Mapping]
	WHERE
		CustomerID = @CustomerID and CustomerRoleID=@CustomerRoleID
END
