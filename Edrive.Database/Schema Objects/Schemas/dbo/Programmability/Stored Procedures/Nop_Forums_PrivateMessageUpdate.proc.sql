

CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageUpdate]
(
	@PrivateMessageID int,
	@FromUserID int,
	@ToUserID int,
	@Subject nvarchar(450),
	@Text nvarchar(max),
	@IsRead bit,
	@IsDeletedByAuthor bit,
	@IsDeletedByRecipient bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Forums_PrivateMessage]
	SET
		[FromUserID]=@FromUserID,
		[ToUserID]=@ToUserID,
		[Subject]=@Subject,
		[Text]=@Text,
		[IsRead]=@IsRead,
		[IsDeletedByAuthor]=@IsDeletedByAuthor,
		[IsDeletedByRecipient]=@IsDeletedByRecipient,
		[CreatedOn]=@CreatedOn
	WHERE
		PrivateMessageID = @PrivateMessageID
END
