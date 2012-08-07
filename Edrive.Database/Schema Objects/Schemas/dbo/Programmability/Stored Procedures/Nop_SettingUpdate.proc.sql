

CREATE PROCEDURE [dbo].[Nop_SettingUpdate]
(
	@SettingID int,
	@Name nvarchar(200),
	@Value nvarchar(2000),	
	@Description nvarchar(MAX)
)
AS
BEGIN
	UPDATE [Nop_Setting]
	SET
			[Name]=@Name,
			[Value]=@Value,	
			[Description]=@Description
	WHERE
		[SettingID] = @SettingID
END
