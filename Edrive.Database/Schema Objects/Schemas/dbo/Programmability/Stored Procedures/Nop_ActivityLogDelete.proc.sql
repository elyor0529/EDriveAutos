

CREATE PROCEDURE [dbo].[Nop_ActivityLogDelete]
(
	@ActivityLogID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ActivityLog]
	WHERE
		ActivityLogID = @ActivityLogID
END
