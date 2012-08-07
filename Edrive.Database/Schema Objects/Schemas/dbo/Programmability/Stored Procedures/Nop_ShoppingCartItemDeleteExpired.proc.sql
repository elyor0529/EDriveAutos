

CREATE PROCEDURE [dbo].[Nop_ShoppingCartItemDeleteExpired]
(
	@OlderThan datetime
)
AS
BEGIN
	SET NOCOUNT ON
		
	DELETE FROM [Nop_ShoppingCartItem]
	WHERE UpdatedOn < @OlderThan
END
