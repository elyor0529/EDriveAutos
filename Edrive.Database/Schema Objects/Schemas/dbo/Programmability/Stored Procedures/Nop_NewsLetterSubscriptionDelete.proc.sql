

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionDelete]
(
	@NewsLetterSubscriptionID int
)
AS
BEGIN
	DELETE FROM
		[Nop_NewsLetterSubscription]
	WHERE
		NewsLetterSubscriptionID = @NewsLetterSubscriptionID
END
