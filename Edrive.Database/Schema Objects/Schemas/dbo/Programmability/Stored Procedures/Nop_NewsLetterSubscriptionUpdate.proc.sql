

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionUpdate]
(
	@NewsLetterSubscriptionID int,
	@NewsLetterSubscriptionGuid uniqueidentifier,
	@Email nvarchar(255),
	@Active bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE
		[Nop_NewsLetterSubscription]
	SET
		NewsLetterSubscriptionGuid = @NewsLetterSubscriptionGuid,
		Email = @Email,
		Active = @Active,
		CreatedOn = @CreatedOn
	WHERE
		NewsLetterSubscriptionID = @NewsLetterSubscriptionID
END
