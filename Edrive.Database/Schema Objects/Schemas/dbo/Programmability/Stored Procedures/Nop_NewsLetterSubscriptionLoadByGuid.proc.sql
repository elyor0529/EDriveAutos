

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadByGuid]
(
	@NewsLetterSubscriptionGuid uniqueidentifier
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		* 
	FROM
		[Nop_NewsLetterSubscription]
	WHERE
		NewsLetterSubscriptionGuid = @NewsLetterSubscriptionGuid
END
