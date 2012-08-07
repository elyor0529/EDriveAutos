

CREATE PROCEDURE [dbo].[Nop_CustomerRoleLoadByCustomerID]
	@ShowHidden bit = 0,
	@CustomerID int = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT cr.*
	FROM [Nop_CustomerRole] cr
	INNER JOIN Nop_Customer_CustomerRole_Mapping crm
	ON cr.CustomerRoleID = crm.CustomerRoleID
	WHERE (cr.Active = 1 or @ShowHidden = 1) and cr.Deleted=0 and crm.CustomerID=@CustomerID
	order by [Name]
END
