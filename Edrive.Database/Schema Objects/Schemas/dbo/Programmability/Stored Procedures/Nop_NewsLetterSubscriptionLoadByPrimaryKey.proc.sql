

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadByPrimaryKey]
(
	@NewsLetterSubscriptionID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		* 
	FROM
		[Nop_NewsLetterSubscription]
	WHERE
		NewsLetterSubscriptionID = @NewsLetterSubscriptionID
END
