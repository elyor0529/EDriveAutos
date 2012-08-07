

CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadByPrimaryKey]
(
	@PrivateMessageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Forums_PrivateMessage]
	WHERE
		(PrivateMessageID = @PrivateMessageID)
END
