

CREATE PROCEDURE [dbo].[Nop_ActivityLogTypeInsert]
(
	@ActivityLogTypeID int = NULL output,
	@SystemKeyword nvarchar(50),
    @Name nvarchar(100),
    @Enabled bit
)
AS
BEGIN
	INSERT
	INTO [Nop_ActivityLogType]
	(
		[SystemKeyword],
		[Name],
		[Enabled]
	)
	VALUES
	(
		@SystemKeyword,
		@Name,
		@Enabled
	)

	set @ActivityLogTypeID=SCOPE_IDENTITY()
END
