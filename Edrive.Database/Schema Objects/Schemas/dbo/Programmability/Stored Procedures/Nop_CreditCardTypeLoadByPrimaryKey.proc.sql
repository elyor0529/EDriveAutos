

CREATE PROCEDURE [dbo].[Nop_CreditCardTypeLoadByPrimaryKey]
(
	@CreditCardTypeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_CreditCardType]
	WHERE
		CreditCardTypeID=@CreditCardTypeID
END
