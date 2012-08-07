

CREATE PROCEDURE [dbo].[Nop_PaymentMethodDelete]
(
	@PaymentMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM Nop_PaymentMethod
	WHERE
		 PaymentMethodID = @PaymentMethodID
END
