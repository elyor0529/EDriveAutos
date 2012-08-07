

CREATE PROCEDURE [dbo].[Nop_ACLUpdate]
(
	@ACLID int,
	@CustomerActionID int,
	@CustomerRoleID int,
	@Allow bit
)
AS
BEGIN

	UPDATE [Nop_ACL]
	SET
		[CustomerActionID]=@CustomerActionID,
		[CustomerRoleID]=@CustomerRoleID,
		[Allow]=@Allow
	WHERE
		[ACLID]=@ACLID

END
