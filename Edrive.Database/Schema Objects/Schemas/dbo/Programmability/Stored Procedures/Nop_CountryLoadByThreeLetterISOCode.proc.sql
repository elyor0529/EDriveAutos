

CREATE PROCEDURE [dbo].[Nop_CountryLoadByThreeLetterISOCode]
(
	@ThreeLetterISOCode nvarchar(3)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Country]
	WHERE
		(ThreeLetterISOCode = @ThreeLetterISOCode)
END
