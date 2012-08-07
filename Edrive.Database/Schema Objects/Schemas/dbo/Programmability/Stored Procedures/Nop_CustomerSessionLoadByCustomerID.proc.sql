

CREATE PROCEDURE [dbo].[Nop_CustomerSessionLoadByCustomerID]
(
	@CustomerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerSession]
	WHERE
		([CustomerID] = @CustomerID)
	order by LastAccessed desc
END
