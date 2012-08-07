

CREATE PROCEDURE [dbo].[Nop_ACLInsert]
(
	@ACLID int = NULL output,
	@CustomerActionID int,
	@CustomerRoleID int,
	@Allow bit
)
AS
BEGIN
	INSERT
	INTO [Nop_ACL]
	(
		[CustomerActionID],
		[CustomerRoleID],
		[Allow]
	)
	VALUES
	(
		@CustomerActionID,
		@CustomerRoleID,
		@Allow
	)

	set @ACLID=SCOPE_IDENTITY()
END
