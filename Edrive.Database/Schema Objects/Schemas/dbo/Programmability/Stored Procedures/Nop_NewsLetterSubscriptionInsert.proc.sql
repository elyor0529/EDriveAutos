

CREATE PROCEDURE [dbo].[Nop_NewsLetterSubscriptionInsert]
(
	@NewsLetterSubscriptionID int = NULL output,
	@NewsLetterSubscriptionGuid uniqueidentifier,
	@Email nvarchar(255),
	@Active bit,
	@CreatedOn datetime
)
AS
BEGIN
	IF(NOT EXISTS(SELECT * FROM [Nop_NewsLetterSubscription] WHERE Email = @Email))
	BEGIN
		INSERT INTO 
			[Nop_NewsLetterSubscription]
			(
				NewsLetterSubscriptionGuid,
				Email,
				Active,
				CreatedOn
			)
		VALUES
			(
				@NewsLetterSubscriptionGuid,
				@Email,
				@Active,
				@CreatedOn
			)

		SET @NewsLetterSubscriptionID = SCOPE_IDENTITY()
	END
END
