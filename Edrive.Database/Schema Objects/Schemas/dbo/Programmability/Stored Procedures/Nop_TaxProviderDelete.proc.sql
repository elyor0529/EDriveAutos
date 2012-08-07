

CREATE PROCEDURE [dbo].[Nop_TaxProviderDelete]
(
	@TaxProviderID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM Nop_TaxProvider
	WHERE
		 TaxProviderID = @TaxProviderID
END
