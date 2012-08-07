

CREATE PROCEDURE [dbo].[Nop_CountryLoadByPrimaryKey]
(
	@CountryId int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Country]
	WHERE
		([CountryId] = @CountryId)
END
