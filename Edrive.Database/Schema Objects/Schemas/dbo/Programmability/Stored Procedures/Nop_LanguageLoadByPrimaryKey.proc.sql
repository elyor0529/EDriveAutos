

CREATE PROCEDURE [dbo].[Nop_LanguageLoadByPrimaryKey]
(
	@LanguageId int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Language]
	WHERE
		([LanguageId] = @LanguageId)
END
