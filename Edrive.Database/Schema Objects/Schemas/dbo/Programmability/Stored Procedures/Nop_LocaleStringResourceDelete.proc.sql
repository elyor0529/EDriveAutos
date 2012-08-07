

CREATE PROCEDURE [dbo].[Nop_LocaleStringResourceDelete]
(
	@LocaleStringResourceID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_LocaleStringResource]
	WHERE
		LocaleStringResourceID = @LocaleStringResourceID
END
