

CREATE PROCEDURE [dbo].[Nop_ActivityLogLoadByPrimaryKey]
(
	@ActivityLogID int
)
AS
BEGIN

	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ActivityLog]
	WHERE
		ActivityLogID = @ActivityLogID
END
