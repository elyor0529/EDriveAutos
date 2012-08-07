CREATE PROCEDURE [dbo].[Nop_ProductLoadAllPaged]
(
	@CategoryID			int = 0,
	@ManufacturerID		int = 0,
	@ProductTagID		int = 0,
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			money = null,
	@PriceMax			money = null,
	@Keywords			nvarchar(MAX),	
	@SearchDescriptions bit = 0,
	@ShowHidden			bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@FilteredSpecs		nvarchar(300) = null,	--filter by attributes (comma-separated list). e.g. 14,15,16
	@LanguageID			int = 0,
	@OrderBy			int = 0, --0 position, 5 - Name, 10 - Price
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--init
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

	SET @PriceMin = isnull(@PriceMin, 0)
	SET @PriceMax = isnull(@PriceMax, 2147483644)
	
	--filter by attributes
	SET @FilteredSpecs = isnull(@FilteredSpecs, '')
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionID int not null
	)
	INSERT INTO #FilteredSpecs (SpecificationAttributeOptionID)
	SELECT CAST(data as int) FROM [NOP_splitstring_to_table](@FilteredSpecs, ',');
	
	DECLARE @SpecAttributesCount int	
	SELECT @SpecAttributesCount = COUNT(1) FROM #FilteredSpecs

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #DisplayOrderTmp 
	(
		[ID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL,
		[Name] nvarchar(400) not null,
		[Price] money not null,
		[DisplayOrder1] int,
		[DisplayOrder2] int,
	)

	INSERT INTO #DisplayOrderTmp ([ProductID], [Name], [Price], [DisplayOrder1], [DisplayOrder2])
	SELECT p.ProductID, p.Name, pv.Price, pcm.DisplayOrder, pmm.DisplayOrder 
	FROM Nop_Product p with (NOLOCK) 
	LEFT OUTER JOIN Nop_Product_Category_Mapping pcm with (NOLOCK) ON p.ProductID=pcm.ProductID
	LEFT OUTER JOIN Nop_Product_Manufacturer_Mapping pmm with (NOLOCK) ON p.ProductID=pmm.ProductID
	LEFT OUTER JOIN Nop_ProductTag_Product_Mapping ptpm with (NOLOCK) ON p.ProductID=ptpm.ProductID
	LEFT OUTER JOIN Nop_ProductVariant pv with (NOLOCK) ON p.ProductID = pv.ProductID
	LEFT OUTER JOIN Nop_ProductVariantLocalized pvl with (NOLOCK) ON pv.ProductVariantID = pvl.ProductVariantID AND pvl.LanguageID = @LanguageID
	LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE 
		(
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				@ShowHidden = 1 OR pv.Published = 1
			)
		AND 
			(
				p.Deleted=0
			)
		AND (
				@CategoryID IS NULL OR @CategoryID=0
				OR (pcm.CategoryID=@CategoryID AND (@FeaturedProducts IS NULL OR pcm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ManufacturerID IS NULL OR @ManufacturerID=0
				OR (pmm.ManufacturerID=@ManufacturerID AND (@FeaturedProducts IS NULL OR pmm.IsFeaturedProduct=@FeaturedProducts))
			)
		AND (
				@ProductTagID IS NULL OR @ProductTagID=0
				OR ptpm.ProductTagID=@ProductTagID
			)
		AND (
				pv.Price BETWEEN @PriceMin AND @PriceMax
			)
		AND	(
				-- search standard content
				patindex(@Keywords, isnull(p.name, '')) > 0
				or patindex(@Keywords, isnull(pv.name, '')) > 0
				or patindex(@Keywords, isnull(pv.sku , '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(p.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pv.Description, '')) > 0)					
				-- search language content
				or patindex(@Keywords, isnull(pl.name, '')) > 0
				or patindex(@Keywords, isnull(pvl.name, '')) > 0
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.ShortDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pl.FullDescription, '')) > 0)
				or (@SearchDescriptions = 1 and patindex(@Keywords, isnull(pvl.Description, '')) > 0)
			)
		AND
			(
				@ShowHidden = 1
				OR
				(getutcdate() between isnull(pv.AvailableStartDateTime, '1/1/1900') and isnull(pv.AvailableEndDateTime, '1/1/2999'))
			)
		AND
			(
				--filter by specs
				@SpecAttributesCount = 0
				OR
				(
					NOT EXISTS(
						SELECT 1 
						FROM #FilteredSpecs [fs]
						WHERE [fs].SpecificationAttributeOptionID NOT IN (
							SELECT psam.SpecificationAttributeOptionID
							FROM Nop_Product_SpecificationAttribute_Mapping psam
							WHERE psam.AllowFiltering = 1 AND psam.ProductID = p.ProductID
							)
						)
					
				)
			)
		)
	ORDER BY 
		CASE WHEN @OrderBy = 0 AND @CategoryID IS NOT NULL AND @CategoryID > 0
		THEN pcm.DisplayOrder END,
		CASE WHEN @OrderBy = 0 AND @ManufacturerID IS NOT NULL AND @ManufacturerID > 0
		THEN pmm.DisplayOrder END,
		CASE WHEN @OrderBy = 0
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 5
		THEN dbo.NOP_getnotnullnotempty(pl.[Name],p.[Name]) END,
		CASE WHEN @OrderBy = 10
		THEN pv.Price END

	CREATE TABLE #PageIndex 
	(
		[IndexID] int IDENTITY (1, 1) NOT NULL,
		[ProductID] int NOT NULL
	)

	INSERT INTO #PageIndex ([ProductID])
	SELECT ProductID
	FROM #DisplayOrderTmp with (NOLOCK)
	GROUP BY ProductID
	ORDER BY min([ID])

	--total records
	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	--return
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
		p.[TypeAttribute],
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
		p.[SavingAmount],
		p.[City] ,
		p.[StateID],
		p.[Date_in_Stock],
		p.[FileName],
		p.[IsNew],
		p.[QualifyPrice],p.[OwnerDetail],p.[Show_on_Dealer] ,p.[Offer],
		p.[City_Fuel] ,p.[Highway_Fuel],p.[AverageRetailPrice] ,p.[AverageTradeinPrice],
		p.[SellerName] ,p.[SellerEmail] ,p.[SellerPhone] ,p.[SellerNotes] 
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl on (pl.ProductID = p.ProductID AND pl.LanguageID = @LanguageID) 
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #FilteredSpecs
	DROP TABLE #DisplayOrderTmp
	DROP TABLE #PageIndex
END