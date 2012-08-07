

CREATE PROCEDURE [dbo].[Nop_ACLIsAllowed]
(
	@CustomerID int,
	@ActionSystemKeyword nvarchar(100)
)
AS
BEGIN

	DECLARE @Result bit
	SET @Result=0
	
	IF EXISTS (SELECT 1 FROM [Nop_Customer_CustomerRole_Mapping] crm
				INNER JOIN [Nop_CustomerRole] cr ON crm.CustomerRoleID=cr.CustomerRoleID
				INNER JOIN [Nop_ACL] acl ON cr.CustomerRoleID=acl.CustomerRoleID
				INNER JOIN [Nop_CustomerAction] ca ON acl.CustomerActionID=ca.CustomerActionID
				WHERE 
					crm.CustomerID=@CustomerID 
					AND cr.Deleted=0 
					AND cr.Active=1
					AND acl.Allow=1
					AND ca.SystemKeyword=@ActionSystemKeyword 
				)	
		SET @Result=1
	ELSE
		SET @Result=0

	SELECT @Result as [result]

END
