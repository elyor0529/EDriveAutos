CREATE PROCEDURE [dbo].[Nop_ProductLoadRecentlyAdded] 
(	
	@Number			int,
	@LanguageID		int,
	@ShowHidden		bit = 0
)
AS
BEGIN
    SET NOCOUNT ON
    IF @Number is null or @Number = 0
        SET @Number = 20

	CREATE TABLE #ProductFilter
	(
	    ProductFilterID int IDENTITY (1, 1) NOT NULL,
	    ProductID int not null
	)
	
	INSERT #ProductFilter (ProductID)
	SELECT p.ProductID
	FROM Nop_Product p with (NOLOCK)
	WHERE
		(p.Published = 1 or @ShowHidden = 1) AND
		p.Deleted = 0
	ORDER BY p.CreatedOn desc

	SELECT
		p.[ProductId],
		dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) as [Name],
		dbo.NOP_getnotnullnotempty(pl.[ShortDescription],p.[ShortDescription]) as [ShortDescription],
		dbo.NOP_getnotnullnotempty(pl.[FullDescription],p.[FullDescription]) as [FullDescription],
		p.[AdminComment], 
		p.[ProductTypeID], 
		p.[TemplateID], 
		p.[ShowOnHomePage], 
		dbo.NOP_getnotnullnotempty(pl.[MetaKeywords],p.[MetaKeywords]) as [MetaKeywords],
		dbo.NOP_getnotnullnotempty(pl.[MetaDescription],p.[MetaDescription]) as [MetaDescription],
		dbo.NOP_getnotnullnotempty(pl.[MetaTitle],p.[MetaTitle]) as [MetaTitle],
		dbo.NOP_getnotnullnotempty(pl.[SEName],p.[SEName]) as [SEName],
		p.[AllowCustomerReviews], 
		p.[AllowCustomerRatings], 
		p.[RatingSum], 
		p.[TotalRatingVotes], 
		p.[Published], 
		p.[Deleted], 
		p.[CreatedOn], 
		p.[UpdatedOn],
		p.[VIN],
		p.[CustomerID],
		p.[VehicleName],
		p.[Type],
		p.[TypeAttribute] ,
		p.[Stock],
		p.[Year],
		p.[YearAttribute],
		p.[Make],
		p.[MakeAttribute],
		p.[Model],
		p.[Trim],
		p.[Free_Text],
		p.[Body],
		p.[BodyAttribute],
		p.[Mileage],
		p.[MileageAttribute],
		p.[Price_Current],
		p.[Reserved],
		p.[Price_Wholesale],
		p.[Price_Cost],
		p.[PriceAttribute],
		p.[Title],
		p.[Condition],
		p.[Exterior_Color],
		p.[Interior_Color],
		p.[Doors],
		p.[Engine],
		p.[Transmission],
		p.[Fuel_Type],
		p.[Drive_Type],
		p.[Options],
		p.[Warranty],
		p.[WarrantyAttribute],
		p.[Description],
		p.[Pics],
		p.[Dealer_Name],
		p.[Dealer_Zip],
		p.[Date_in_Stock],
		p.[SavingAmount],
		p.[City] ,
		p.[StateID], 
		p.[FileName],
		p.[IsNew],
		p.[IsFeature],
		p.[QualifyPrice],
		p.[OwnerDetail],
		p.[Show_on_Dealer],
		p.[SellerName] ,
		p.[SellerEmail] ,
		p.[SellerPhone] ,
		p.[SellerNotes] 
		
		 
		 
	FROM 
		Nop_Product p with (NOLOCK)
		inner join #ProductFilter pf with (NOLOCK) ON p.ProductID = pf.ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE pf.ProductFilterID <= @Number
	DROP TABLE #ProductFilter
END