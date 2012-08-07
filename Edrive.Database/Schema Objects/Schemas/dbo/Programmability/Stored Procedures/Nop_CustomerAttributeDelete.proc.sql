

CREATE PROCEDURE [dbo].[Nop_CustomerAttributeDelete]
(
	@CustomerAttributeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerAttribute]
	WHERE
		CustomerAttributeID = @CustomerAttributeID
END
