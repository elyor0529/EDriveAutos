

CREATE PROCEDURE [dbo].[Nop_CreditCardTypeLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CreditCardType]
	where Deleted=0
	order by DisplayOrder
END
