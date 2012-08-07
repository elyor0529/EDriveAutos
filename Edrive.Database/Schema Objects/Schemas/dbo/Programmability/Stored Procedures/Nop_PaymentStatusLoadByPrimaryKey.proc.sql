

CREATE PROCEDURE [dbo].[Nop_PaymentStatusLoadByPrimaryKey]
(
	@PaymentStatusID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PaymentStatus]
	WHERE
		PaymentStatusID = @PaymentStatusID
END
