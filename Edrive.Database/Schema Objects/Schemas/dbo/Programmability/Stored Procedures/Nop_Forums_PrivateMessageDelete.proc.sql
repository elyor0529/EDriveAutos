

CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageDelete]
(
	@PrivateMessageID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Forums_PrivateMessage]
	WHERE
		PrivateMessageID = @PrivateMessageID
END
