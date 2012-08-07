

CREATE PROCEDURE [dbo].[Nop_PollLoadByPrimaryKey]
(
	@PollID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Poll]
	WHERE
		PollID = @PollID
END
