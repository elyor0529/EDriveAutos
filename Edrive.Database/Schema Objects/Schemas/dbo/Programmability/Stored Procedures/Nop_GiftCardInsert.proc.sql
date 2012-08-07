

CREATE PROCEDURE [dbo].[Nop_GiftCardInsert]
(
	@GiftCardID int = NULL output,
	@PurchasedOrderProductVariantID int,
	@Amount money,
	@IsGiftCardActivated bit,
	@GiftCardCouponCode nvarchar(100),
	@RecipientName nvarchar(100),
	@RecipientEmail nvarchar(100),
	@SenderName nvarchar(100),
	@SenderEmail nvarchar(100),
	@Message nvarchar(4000),
	@IsRecipientNotified bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_GiftCard]
	(
		[PurchasedOrderProductVariantID],
		[Amount],
		[IsGiftCardActivated],
		[GiftCardCouponCode],
		[RecipientName],
		[RecipientEmail],
		[SenderName],
		[SenderEmail],
		[Message],
		[IsRecipientNotified],
		[CreatedOn]
	)
	VALUES
	(
		@PurchasedOrderProductVariantID,
		@Amount,
		@IsGiftCardActivated,
		@GiftCardCouponCode,
		@RecipientName,
		@RecipientEmail,
		@SenderName,
		@SenderEmail,
		@Message,
		@IsRecipientNotified,
		@CreatedOn
	)

	set @GiftCardID=SCOPE_IDENTITY()
END
