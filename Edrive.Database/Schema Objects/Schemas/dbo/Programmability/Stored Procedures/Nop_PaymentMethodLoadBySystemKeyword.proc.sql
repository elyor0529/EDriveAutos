

CREATE PROCEDURE [dbo].[Nop_PaymentMethodLoadBySystemKeyword]
(
	@SystemKeyword nvarchar(500)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_PaymentMethod]
	WHERE
		SystemKeyword = @SystemKeyword
END
