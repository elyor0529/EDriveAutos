

CREATE PROCEDURE [dbo].[Nop_CustomerAttributeLoadByPrimaryKey]
(
	@CustomerAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerAttribute]
	WHERE
		CustomerAttributeID = @CustomerAttributeID
END
