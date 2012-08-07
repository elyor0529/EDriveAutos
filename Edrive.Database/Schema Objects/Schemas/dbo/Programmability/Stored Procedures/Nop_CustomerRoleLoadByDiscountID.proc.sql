

CREATE PROCEDURE [dbo].[Nop_CustomerRoleLoadByDiscountID]
(
	@DiscountID int,
	@ShowHidden bit = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		cr.*
	FROM [Nop_CustomerRole] cr
	INNER JOIN [Nop_CustomerRole_Discount_Mapping] crm
	ON cr.CustomerRoleID = crm.CustomerRoleID
	WHERE (cr.Active = 1 or @ShowHidden = 1) and cr.Deleted=0 and crm.DiscountID=@DiscountID
	order by cr.Name
END
