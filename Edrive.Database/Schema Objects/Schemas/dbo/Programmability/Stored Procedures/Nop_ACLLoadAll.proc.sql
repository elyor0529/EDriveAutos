

CREATE PROCEDURE [dbo].[Nop_ACLLoadAll]
	@CustomerActionID int,
	@CustomerRoleID int,
	@Allow int = NULL	
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ACL]
	WHERE 
		(@CustomerActionID IS NULL or @CustomerActionID=0 or [CustomerActionID] = @CustomerActionID)
		AND
		(@CustomerRoleID IS NULL or @CustomerRoleID=0 or [CustomerRoleID] = @CustomerRoleID)
		AND
		(@Allow IS NULL OR [Allow]=@Allow)
	ORDER BY [ACLID] desc
END
