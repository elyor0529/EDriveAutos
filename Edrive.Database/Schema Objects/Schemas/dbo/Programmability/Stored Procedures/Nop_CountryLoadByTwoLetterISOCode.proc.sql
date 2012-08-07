

CREATE PROCEDURE [dbo].[Nop_CountryLoadByTwoLetterISOCode]
(
	@TwoLetterISOCode nvarchar(2)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Country]
	WHERE
		(TwoLetterISOCode = @TwoLetterISOCode)
END
