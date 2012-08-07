

CREATE PROCEDURE [dbo].[Nop_SettingLoadByPrimaryKey]
(
	@SettingId int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Setting]
	WHERE
		([SettingId] = @SettingId)
END
