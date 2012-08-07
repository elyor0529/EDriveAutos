

CREATE PROCEDURE [dbo].[Nop_CustomerLoadByPrimaryKey]
(
	@CustomerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([CustomerID] = @CustomerID)
END
