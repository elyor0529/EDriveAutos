

CREATE PROCEDURE [dbo].[Nop_ShippingMethodLoadByPrimaryKey]
(
	@ShippingMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingMethod]
	WHERE
		ShippingMethodID = @ShippingMethodID
END
