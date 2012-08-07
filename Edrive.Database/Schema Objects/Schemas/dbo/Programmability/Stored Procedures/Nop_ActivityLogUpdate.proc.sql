

CREATE PROCEDURE [dbo].[Nop_ActivityLogUpdate]
(
	@ActivityLogID int,
	@ActivityLogTypeID int,
    @CustomerID int,
    @Comment nvarchar(4000),
    @CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_ActivityLog]
	SET
			[ActivityLogTypeID] = @ActivityLogTypeID,
			[CustomerID] = @CustomerID,
			[Comment] = @Comment,
			[CreatedOn] = @CreatedOn
	WHERE
		ActivityLogID = @ActivityLogID
END
