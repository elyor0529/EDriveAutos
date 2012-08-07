

CREATE PROCEDURE [dbo].[Nop_OrderProductVariantUpdate]
(
	@OrderProductVariantID int,
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

	UPDATE [Nop_OrderProductVariant]
	SET		
		OrderProductVariantGUID=@OrderProductVariantGUID,
		OrderID=@OrderID,
		ProductVariantID=@ProductVariantID,
		UnitPriceInclTax=@UnitPriceInclTax,
		UnitPriceExclTax = @UnitPriceExclTax,
		PriceInclTax=@PriceInclTax,
		PriceExclTax=@PriceExclTax,
		UnitPriceInclTaxInCustomerCurrency=@UnitPriceInclTaxInCustomerCurrency,
		UnitPriceExclTaxInCustomerCurrency=@UnitPriceExclTaxInCustomerCurrency,
		PriceInclTaxInCustomerCurrency=@PriceInclTaxInCustomerCurrency,
		PriceExclTaxInCustomerCurrency=@PriceExclTaxInCustomerCurrency,
		AttributeDescription=@AttributeDescription,
		AttributesXML=@AttributesXML,
		Quantity=@Quantity,
		DiscountAmountInclTax=@DiscountAmountInclTax,
		DiscountAmountExclTax=@DiscountAmountExclTax,
		DownloadCount=@DownloadCount,
		IsDownloadActivated=@IsDownloadActivated,
		LicenseDownloadID=@LicenseDownloadID
	WHERE
		OrderProductVariantID = @OrderProductVariantID
END
