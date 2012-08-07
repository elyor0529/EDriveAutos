

CREATE PROCEDURE [dbo].[Nop_PaymentMethodLoadAll]
(
	@ShowHidden bit = 0,
	@FilterByCountryID int = NULL
)
AS
BEGIN
	SET NOCOUNT ON
	IF(@FilterByCountryID IS NOT NULL AND @FilterByCountryID != 0)
		BEGIN
			SELECT  
				pm.*
		    FROM 
				[Nop_PaymentMethod] pm
		    WHERE 
                pm.PaymentMethodID NOT IN 
				(
				    SELECT 
						pmc.PaymentMethodID
				    FROM 
						[Nop_PaymentMethod_RestrictedCountries] pmc
				    WHERE 
						pmc.CountryID = @FilterByCountryID AND 
						pm.PaymentMethodID = pmc.PaymentMethodID
				)
				AND
				(IsActive = 1 or @ShowHidden = 1)
		   ORDER BY 
				pm.DisplayOrder
		END
	ELSE
		BEGIN
			SELECT 
				*
			FROM 
				[Nop_PaymentMethod]
			WHERE 
				(IsActive = 1 or @ShowHidden = 1)
			ORDER BY 
				DisplayOrder
		END
END
