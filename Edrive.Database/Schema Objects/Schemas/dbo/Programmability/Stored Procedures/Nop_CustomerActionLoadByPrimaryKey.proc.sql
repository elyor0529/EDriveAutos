

CREATE PROCEDURE [dbo].[Nop_CustomerActionLoadByPrimaryKey]
(
	@CustomerActionID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerAction]
	WHERE
		[CustomerActionID] = @CustomerActionID
END
