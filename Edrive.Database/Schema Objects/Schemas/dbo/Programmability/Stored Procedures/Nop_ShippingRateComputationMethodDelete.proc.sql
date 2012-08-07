

CREATE PROCEDURE [dbo].[Nop_ShippingRateComputationMethodDelete]
(
	@ShippingRateComputationMethodID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM Nop_ShippingRateComputationMethod
	WHERE
		 ShippingRateComputationMethodID = @ShippingRateComputationMethodID
END
