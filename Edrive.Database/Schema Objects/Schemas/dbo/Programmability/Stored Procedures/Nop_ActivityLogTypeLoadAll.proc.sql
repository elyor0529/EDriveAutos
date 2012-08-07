

CREATE PROCEDURE [dbo].[Nop_ActivityLogTypeLoadAll]
AS
BEGIN

	SET NOCOUNT ON
	SELECT *
	FROM [Nop_ActivityLogType]
	ORDER BY [Name]
END
