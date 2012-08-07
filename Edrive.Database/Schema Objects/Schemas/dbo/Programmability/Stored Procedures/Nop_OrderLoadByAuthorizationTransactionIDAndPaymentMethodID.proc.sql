

CREATE PROCEDURE [dbo].[Nop_OrderLoadByAuthorizationTransactionIDAndPaymentMethodID]
(
	@AuthorizationTransactionID nvarchar(4000),
	@PaymentMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Order]
	WHERE
		AuthorizationTransactionID=@AuthorizationTransactionID and
		PaymentMethodID=@PaymentMethodID
	ORDER BY CreatedOn desc
END
