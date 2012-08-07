

CREATE PROCEDURE [dbo].[Nop_ActivityLogTypeLoadByPrimaryKey]
(
	@ActivityLogTypeID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ActivityLogType]
	WHERE
		ActivityLogTypeID = @ActivityLogTypeID
END
