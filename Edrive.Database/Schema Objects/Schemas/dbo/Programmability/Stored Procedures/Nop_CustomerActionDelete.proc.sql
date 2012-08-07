

CREATE PROCEDURE [dbo].[Nop_CustomerActionDelete]
(
	@CustomerActionID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_CustomerAction]
	WHERE
		[CustomerActionID] = @CustomerActionID
END
