

CREATE PROCEDURE [dbo].[Nop_CustomerLoadByCustomerRoleID]
	@ShowHidden bit = 0,
	@CustomerRoleID int = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT c.*
	FROM [Nop_Customer] c
	INNER JOIN Nop_Customer_CustomerRole_Mapping crm
	ON c.CustomerID = crm.CustomerID
	WHERE (c.Active = 1 or @ShowHidden = 1) and c.Deleted=0 and crm.CustomerRoleID=@CustomerRoleID
	order by [RegistrationDate] desc
END
