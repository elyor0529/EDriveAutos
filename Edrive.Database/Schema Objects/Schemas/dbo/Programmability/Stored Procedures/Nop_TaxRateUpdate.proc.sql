

CREATE PROCEDURE [dbo].[Nop_TaxRateUpdate]
(
	@TaxRateID int,
	@TaxCategoryID int,
	@CountryID int,
	@StateProvinceID int,
	@Zip nvarchar(50),
	@Percentage decimal(18,4)
)
AS
BEGIN
	UPDATE [Nop_TaxRate]
	SET
		TaxCategoryID=@TaxCategoryID,
		CountryID=@CountryID,
		StateProvinceID=@StateProvinceID,
		Zip=@Zip,
		Percentage=@Percentage
	WHERE
		TaxRateID = @TaxRateID
END
