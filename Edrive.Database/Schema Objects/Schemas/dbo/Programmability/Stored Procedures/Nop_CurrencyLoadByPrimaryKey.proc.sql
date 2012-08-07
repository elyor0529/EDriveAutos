

CREATE PROCEDURE [dbo].[Nop_CurrencyLoadByPrimaryKey]
(
	@CurrencyID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Currency]
	WHERE
		CurrencyID = @CurrencyID
END
