

CREATE PROCEDURE [dbo].[Nop_AddressLoadByCustomerID]
(
	@CustomerID int,
	@GetBillingAddresses bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Address]
	WHERE
		CustomerID=@CustomerID and IsBillingAddress= @GetBillingAddresses
	ORDER BY CreatedOn desc
END
