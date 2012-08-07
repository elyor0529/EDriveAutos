

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantInsert]
(
	@OrderProductVariantID int = NULL output,
	@OrderProductVariantGUID uniqueidentifier,
	@OrderID int,
	@ProductVariantID int,
	@UnitPriceInclTax money,
	@UnitPriceExclTax money,
	@PriceInclTax money,
	@PriceExclTax money,
	@UnitPriceInclTaxInCustomerCurrency money,
	@UnitPriceExclTaxInCustomerCurrency money,
	@PriceInclTaxInCustomerCurrency money,
	@PriceExclTaxInCustomerCurrency money,
	@AttributeDescription nvarchar(4000),
	@AttributesXML xml,
	@Quantity int,
	@DiscountAmountInclTax decimal (18, 4),
	@DiscountAmountExclTax decimal (18, 4),
	@DownloadCount int,
	@IsDownloadActivated bit,
	@LicenseDownloadID int
)
AS
BEGIN
	INSERT
	INTO [Nop_OrderProductVariant]
	(
		OrderProductVariantGUID,
		OrderID,
		ProductVariantID,
		UnitPriceInclTax,
		UnitPriceExclTax,
		PriceInclTax,
		PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency,
		AttributeDescription,
		AttributesXML,
		Quantity,
		DiscountAmountInclTax,
		DiscountAmountExclTax,
		DownloadCount,
		IsDownloadActivated,
		LicenseDownloadID
	)
	VALUES
	(
		@OrderProductVariantGUID,
		@OrderID,
		@ProductVariantID,
		@UnitPriceInclTax,
		@UnitPriceExclTax,
		@PriceInclTax,
		@PriceExclTax,
		@UnitPriceInclTaxInCustomerCurrency,
		@UnitPriceExclTaxInCustomerCurrency,
		@PriceInclTaxInCustomerCurrency,
		@PriceExclTaxInCustomerCurrency,
		@AttributeDescription,
		@AttributesXML,
		@Quantity,
		@DiscountAmountInclTax,
		@DiscountAmountExclTax,
		@DownloadCount,
		@IsDownloadActivated,
		@LicenseDownloadID
	)

	set @OrderProductVariantID=SCOPE_IDENTITY()
END
