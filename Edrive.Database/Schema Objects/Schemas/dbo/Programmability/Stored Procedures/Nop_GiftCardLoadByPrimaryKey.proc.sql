

CREATE PROCEDURE [dbo].[Nop_GiftCardLoadByPrimaryKey]
(
	@GiftCardID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_GiftCard]
	WHERE
		GiftCardID = @GiftCardID
END
