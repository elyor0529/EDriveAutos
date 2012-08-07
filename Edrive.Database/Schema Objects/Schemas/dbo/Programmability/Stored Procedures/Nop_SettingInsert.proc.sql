

CREATE PROCEDURE [dbo].[Nop_SettingInsert]
(
	@SettingId int = NULL output,
	@Name nvarchar(200),
	@Value nvarchar(2000),	
	@Description nvarchar(MAX)
)
AS
BEGIN
	INSERT
	INTO [Nop_Setting]
	(
			[Name],
			[Value],	
			[Description]
	)
	VALUES
	(
			@Name,
			@Value,	
			@Description
	)

	set @SettingId=SCOPE_IDENTITY()
END
