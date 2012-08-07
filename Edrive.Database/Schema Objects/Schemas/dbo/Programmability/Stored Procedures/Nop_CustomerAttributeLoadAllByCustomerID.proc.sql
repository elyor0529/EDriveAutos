

CREATE PROCEDURE [dbo].[Nop_CustomerAttributeLoadAllByCustomerID]
(
	@CustomerID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CustomerAttribute]
	WHERE
		(CustomerID = @CustomerID)
END
