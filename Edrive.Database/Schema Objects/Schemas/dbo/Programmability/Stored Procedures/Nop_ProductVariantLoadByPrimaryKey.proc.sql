

CREATE PROCEDURE [dbo].[Nop_ProductVariantLoadByPrimaryKey]
(
	@ProductVariantID int,
	@LanguageID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT	 
		pv.ProductVariantId, 
		pv.ProductID, 
		dbo.NOP_getnotnullnotempty(pvl.[Name],pv.[Name]) as [Name], 
		pv.SKU, 
		dbo.NOP_getnotnullnotempty(pvl.[Description],pv.[Description]) as [Description], 
		pv.AdminComment, 
		pv.ManufacturerPartNumber, 
		pv.IsGiftCard, 
		pv.IsDownload, 
		pv.DownloadID,                      
		pv.UnlimitedDownloads, 
		pv.MaxNumberOfDownloads, 
		pv.DownloadExpirationDays, 
		pv.DownloadActivationType, 
		pv.HasSampleDownload, 
		pv.SampleDownloadID,                       
		pv.HasUserAgreement, 
		pv.UserAgreementText, 
		pv.IsRecurring, 
		pv.CycleLength, 
		pv.CyclePeriod,
		pv.TotalCycles, 
		pv.IsShipEnabled, 
		pv.IsFreeShipping, 
		pv.AdditionalShippingCharge, 
		pv.IsTaxExempt, 
		pv.TaxCategoryID, 
		pv.ManageInventory, 
		pv.StockQuantity, 
		pv.DisplayStockAvailability, 
		pv.MinStockQuantity,                       
		pv.LowStockActivityID, 
		pv.NotifyAdminForQuantityBelow, 
		pv.AllowOutOfStockOrders, 
		pv.OrderMinimumQuantity, 
		pv.OrderMaximumQuantity, 
		pv.WarehouseID, 
		pv.DisableBuyButton, 
		pv.Price, 
		pv.OldPrice, 
		pv.ProductCost,
		pv.CustomerEntersPrice,
		pv.MinimumCustomerEnteredPrice,
		pv.MaximumCustomerEnteredPrice,
		pv.Weight, 
		pv.Length, 
		pv.Width, 
		pv.Height, 
		pv.PictureID, 
		pv.AvailableStartDateTime, 
		pv.AvailableEndDateTime, 
		pv.Published,                      
		pv.Deleted, 
		pv.DisplayOrder, 
		pv.CreatedOn, 
		pv.UpdatedOn
	FROM [Nop_ProductVariant] pv
		LEFT OUTER JOIN [Nop_ProductVariantLocalized] pvl 
		ON pvl.ProductVariantId = pv.ProductVariantId AND pvl.LanguageID = @LanguageID
	WHERE
		(pv.ProductVariantID = @ProductVariantID)
END
