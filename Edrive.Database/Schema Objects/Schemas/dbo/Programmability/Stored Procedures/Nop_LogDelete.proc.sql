

CREATE PROCEDURE [dbo].[Nop_LogDelete]
(
	@LogID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Log]
	WHERE
		LogID = @LogID
END
