

CREATE PROCEDURE [dbo].[Nop_PaymentMethodLoadByPrimaryKey]
(
	@PaymentMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PaymentMethod]
	WHERE
		PaymentMethodID = @PaymentMethodID
END
