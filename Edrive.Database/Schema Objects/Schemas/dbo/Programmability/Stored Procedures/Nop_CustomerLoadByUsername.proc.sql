

CREATE PROCEDURE [dbo].[Nop_CustomerLoadByUsername]
(
	@Username nvarchar(100)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([Username] = @Username)
	ORDER BY CustomerID
END
