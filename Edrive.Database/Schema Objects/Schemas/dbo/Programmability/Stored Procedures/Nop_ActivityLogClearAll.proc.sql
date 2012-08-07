

CREATE PROCEDURE [dbo].[Nop_ActivityLogClearAll]
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ActivityLog]
END
