

CREATE PROCEDURE [dbo].[Nop_TaxRateInsert]
(
	@TaxRateID int = NULL output,
	@TaxCategoryID int,
	@CountryID int,
	@StateProvinceID int,
	@Zip nvarchar(50),
	@Percentage decimal(18,4)
)
AS
BEGIN
	INSERT
	INTO [Nop_TaxRate]
	(
		[TaxCategoryID],
		[CountryID],
		[StateProvinceID],
		[Zip],
		[Percentage]
	)
	VALUES
	(
		@TaxCategoryID,
		@CountryID,
		@StateProvinceID,
		@Zip,
		@Percentage
	)

	set @TaxRateID=SCOPE_IDENTITY()
END
