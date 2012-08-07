

CREATE PROCEDURE [dbo].[Nop_LogLoadByPrimaryKey]
(
	@LogID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Log]
	WHERE
		LogID=@LogID
END
