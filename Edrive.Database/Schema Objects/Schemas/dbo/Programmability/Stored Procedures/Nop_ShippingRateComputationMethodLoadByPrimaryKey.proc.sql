

CREATE PROCEDURE [dbo].[Nop_ShippingRateComputationMethodLoadByPrimaryKey]
(
	@ShippingRateComputationMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingRateComputationMethod]
	WHERE
		ShippingRateComputationMethodID = @ShippingRateComputationMethodID
END
