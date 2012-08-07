

CREATE PROCEDURE [dbo].[Nop_CustomerRoleLoadByPrimaryKey]
(
	@CustomerRoleID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerRole]
	WHERE
		CustomerRoleID = @CustomerRoleID
END
