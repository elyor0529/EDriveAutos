CREATE PROCEDURE [dbo].[Nop_ProductVariantInsertNew]
(
    @ProductVariantID int = NULL output,
    @VIN nvarchar(MAX)
  )
AS
BEGIN
    
    Declare @ProductID as int
    select @ProductID = ProductId from Nop_Product where VIN = @VIN
    
    INSERT INTO [Nop_ProductVariant]
    (
        ProductId,[Name],SKU,[Description],AdminComment,ManufacturerPartNumber,
		IsGiftCard,IsDownload,DownloadID,UnlimitedDownloads,MaxNumberOfDownloads,
		DownloadExpirationDays,DownloadActivationType,HasSampleDownload,
		SampleDownloadID,HasUserAgreement,UserAgreementText,IsRecurring,
		CycleLength,CyclePeriod,TotalCycles,IsShipEnabled,IsFreeShipping,
		AdditionalShippingCharge,IsTaxExempt,TaxCategoryID,ManageInventory,
		DisplayStockAvailability,StockQuantity,MinStockQuantity,
        LowStockActivityID,NotifyAdminForQuantityBelow,AllowOutOfStockOrders,
		OrderMinimumQuantity,OrderMaximumQuantity,WarehouseId,DisableBuyButton,
        Price,OldPrice,ProductCost,CustomerEntersPrice,MinimumCustomerEnteredPrice,
		MaximumCustomerEnteredPrice,Weight,[Length],Width,Height,PictureID,
		AvailableStartDateTime,AvailableEndDateTime,Published,Deleted,
        DisplayOrder,CreatedOn,UpdatedOn
    )
    VALUES
    (
        @ProductID,'','','','','','false','false',0,'false',10000,0,1,
		'false',0,'false','','false',0,0,0,'false','false',0,'false',0,
		0,'false',0,0,0,0,'false',0,0,0,'false',0,0,0,'false',0,0,0,0,0,0,0,null,
		null,'true','false',1,getdate(),getdate()
    )
	
    set @ProductVariantID=SCOPE_IDENTITY()
END