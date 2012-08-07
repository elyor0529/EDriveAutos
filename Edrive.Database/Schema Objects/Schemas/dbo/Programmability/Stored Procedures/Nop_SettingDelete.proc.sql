

CREATE PROCEDURE [dbo].[Nop_SettingDelete]
(
	@SettingId int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Setting]
	WHERE
		[SettingId] = @SettingId
END
