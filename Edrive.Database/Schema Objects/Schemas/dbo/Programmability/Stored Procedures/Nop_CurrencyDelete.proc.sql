

CREATE PROCEDURE [dbo].[Nop_CurrencyDelete]
(
	@CurrencyID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Currency]
	WHERE
		CurrencyID = @CurrencyID
END
