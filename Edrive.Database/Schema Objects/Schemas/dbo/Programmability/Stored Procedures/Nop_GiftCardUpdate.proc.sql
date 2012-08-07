

CREATE PROCEDURE [dbo].[Nop_GiftCardUpdate]
(
	@GiftCardID int,
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
	UPDATE [Nop_GiftCard]
	SET
		[PurchasedOrderProductVariantID] = @PurchasedOrderProductVariantID,
		[Amount] = @Amount,
		[IsGiftCardActivated] = @IsGiftCardActivated,
		[GiftCardCouponCode]= @GiftCardCouponCode,
		[RecipientName]=@RecipientName,
		[RecipientEmail]=@RecipientEmail,
		[SenderName]=@SenderName,
		[SenderEmail]=@SenderEmail,
		[Message]=@Message,
		[IsRecipientNotified]=@IsRecipientNotified,
		[CreatedOn] = @CreatedOn
	WHERE
		GiftCardID=@GiftCardID
END
