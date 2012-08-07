

CREATE PROCEDURE [dbo].[Nop_ShippingMethodLoadAll]
(
	@FilterByCountryID int = NULL
)
AS
BEGIN
	SET NOCOUNT ON
	IF(@FilterByCountryID IS NOT NULL AND @FilterByCountryID != 0)
		BEGIN
			SELECT  
				sm.*
		    FROM 
				[Nop_ShippingMethod] sm
		    WHERE 
                sm.ShippingMethodID NOT IN 
				(
				    SELECT 
						smc.ShippingMethodID
				    FROM 
						[Nop_ShippingMethod_RestrictedCountries] smc
				    WHERE 
						smc.CountryID = @FilterByCountryID AND 
						sm.ShippingMethodID = smc.ShippingMethodID
				)
		   ORDER BY 
				sm.DisplayOrder
		END
	ELSE
		BEGIN
			SELECT 
				*
			FROM 
				[Nop_ShippingMethod]
			ORDER BY
				DisplayOrder
		END
END
