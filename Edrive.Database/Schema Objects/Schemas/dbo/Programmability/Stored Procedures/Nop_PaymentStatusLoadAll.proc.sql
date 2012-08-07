

CREATE PROCEDURE [dbo].[Nop_PaymentStatusLoadAll]

AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PaymentStatus]
	ORDER BY PaymentStatusID
END
