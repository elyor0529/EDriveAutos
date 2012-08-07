

CREATE PROCEDURE [dbo].[Nop_StateProvinceLoadByAbbreviation]
(
	@Abbreviation nvarchar(10)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_StateProvince]
	WHERE
		Abbreviation = @Abbreviation
END
