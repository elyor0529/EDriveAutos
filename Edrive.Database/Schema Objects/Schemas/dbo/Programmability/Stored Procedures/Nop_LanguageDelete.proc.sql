

CREATE PROCEDURE [dbo].[Nop_LanguageDelete]
(
	@LanguageId int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Language]
	WHERE
		[LanguageId] = @LanguageId
END
