

CREATE PROCEDURE [dbo].[Nop_GiftCardDelete]
(
	@GiftCardID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_GiftCard]
	WHERE
		GiftCardID = @GiftCardID
END
