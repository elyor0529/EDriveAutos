

CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageInsert]
(
	@PrivateMessageID int = NULL output,
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
	INSERT
	INTO [Nop_Forums_PrivateMessage]
	(
		[FromUserID],
		[ToUserID],
		[Subject],
		[Text],
		[IsRead],
		[IsDeletedByAuthor],
		[IsDeletedByRecipient],
		[CreatedOn]
	)
	VALUES
	(
		@FromUserID,
		@ToUserID,
		@Subject,
		@Text,
		@IsRead,
		@IsDeletedByAuthor,
		@IsDeletedByRecipient,
		@CreatedOn
	)

	set @PrivateMessageID=SCOPE_IDENTITY()
END
