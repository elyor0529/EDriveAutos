

CREATE PROCEDURE [dbo].[Nop_CountryDelete]
(
	@CountryId int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Country]
	WHERE
		[CountryId] = @CountryId
END
