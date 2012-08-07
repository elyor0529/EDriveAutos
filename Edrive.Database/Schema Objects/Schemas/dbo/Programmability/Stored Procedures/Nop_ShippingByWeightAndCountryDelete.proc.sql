

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryDelete]
(
	@ShippingByWeightAndCountryID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_ShippingByWeightAndCountry]
	WHERE
		ShippingByWeightAndCountryID = @ShippingByWeightAndCountryID
END
