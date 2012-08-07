
CREATE PROCEDURE [dbo].[Nop_ActivityLogTypeUpdate]
(
	@ActivityLogTypeID int,
	@SystemKeyword nvarchar(50),
    @Name nvarchar(100),
    @Enabled bit
)
AS
BEGIN
	UPDATE [Nop_ActivityLogType]
	SET
			[SystemKeyword] = @SystemKeyword,
			[Name] = @Name,
			[Enabled] = @Enabled
	WHERE
		ActivityLogTypeID = @ActivityLogTypeID
END
