

CREATE PROCEDURE [dbo].[Nop_CustomerRoleLoadAll]
	@ShowHidden bit = 0
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CustomerRole]
	WHERE (Active = 1 or @ShowHidden = 1) and Deleted=0
	order by [Name]
END
