

CREATE PROCEDURE [dbo].[Nop_LocaleStringResourceLoadByPrimaryKey]
(
	@LocaleStringResourceID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_LocaleStringResource]
	WHERE
		(LocaleStringResourceID = @LocaleStringResourceID)
END
