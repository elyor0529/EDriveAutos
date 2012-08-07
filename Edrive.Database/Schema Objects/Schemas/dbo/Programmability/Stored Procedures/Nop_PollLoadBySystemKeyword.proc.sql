

CREATE PROCEDURE [dbo].[Nop_PollLoadBySystemKeyword]
(
	@SystemKeyword nvarchar(400)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Poll]
	WHERE
		SystemKeyword = @SystemKeyword
END
