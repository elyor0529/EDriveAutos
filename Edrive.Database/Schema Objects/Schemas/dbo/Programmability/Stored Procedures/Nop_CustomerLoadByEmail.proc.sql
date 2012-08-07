CREATE PROCEDURE [dbo].[Nop_CustomerLoadByEmail]
(
	@Email nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Customer]
	WHERE
		([Email] = @Email) and Deleted = 0
	ORDER BY CustomerID
END