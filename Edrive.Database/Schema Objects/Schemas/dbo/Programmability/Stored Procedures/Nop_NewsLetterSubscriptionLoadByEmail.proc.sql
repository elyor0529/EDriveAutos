

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionLoadByEmail]
(
	@Email nvarchar(255)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		* 
	FROM
		[Nop_NewsLetterSubscription]
	WHERE
		Email = @Email
END
