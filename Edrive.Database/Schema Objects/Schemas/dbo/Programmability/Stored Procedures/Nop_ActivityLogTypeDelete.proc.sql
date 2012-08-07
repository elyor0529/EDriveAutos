

CREATE PROCEDURE [dbo].[Nop_ActivityLogTypeDelete]
(
	@ActivityLogTypeID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ActivityLogType]
	WHERE
		ActivityLogTypeID = @ActivityLogTypeID
END
