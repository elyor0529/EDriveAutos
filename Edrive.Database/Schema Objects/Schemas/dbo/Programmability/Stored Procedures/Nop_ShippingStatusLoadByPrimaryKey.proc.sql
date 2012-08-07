

CREATE PROCEDURE [dbo].[Nop_ShippingStatusLoadByPrimaryKey]
(
	@ShippingStatusID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingStatus]
	WHERE
		ShippingStatusID = @ShippingStatusID
END
