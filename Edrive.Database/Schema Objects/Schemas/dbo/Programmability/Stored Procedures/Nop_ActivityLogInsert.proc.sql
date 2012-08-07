

CREATE PROCEDURE [dbo].[Nop_ActivityLogInsert]
(
	@ActivityLogID int = NULL output,
	@ActivityLogTypeID int,
    @CustomerID int,
    @Comment nvarchar(4000),
    @CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_ActivityLog]
	(
		[ActivityLogTypeID],
		[CustomerID],
		[Comment],
		[CreatedOn]
	)
	VALUES
	(
		@ActivityLogTypeID,
		@CustomerID,
		@Comment,
		@CreatedOn
	)

	set @ActivityLogID=SCOPE_IDENTITY()
END
