

CREATE PROCEDURE [dbo].[Nop_ShippingByWeightAndCountryLoadByPrimaryKey]
(
	@ShippingByWeightAndCountryID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_ShippingByWeightAndCountry]
	WHERE
		ShippingByWeightAndCountryID = @ShippingByWeightAndCountryID
END
