

CREATE PROCEDURE [dbo].[Nop_ProductAlsoPurchasedLoadByProductID]
(
	@ProductID			int,
	@LanguageID			int,
	@ShowHidden			bit,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ProductID int NOT NULL,
		ProductsPurchased int NOT NULL,
	)

	INSERT INTO #PageIndex (ProductID, ProductsPurchased)
	SELECT p.ProductID, SUM(opv.Quantity) as ProductsPurchased
	FROM    
		Nop_OrderProductVariant opv WITH (NOLOCK)
		INNER JOIN Nop_ProductVariant pv ON pv.ProductVariantId = opv.ProductVariantId
		INNER JOIN Nop_Product p ON p.ProductId = pv.ProductId
	WHERE
		opv.OrderID IN 
		(
			/* This inner query should retrieve all orders that have contained the productID */
			SELECT 
				DISTINCT OrderID
			FROM 
				Nop_OrderProductVariant opv2 WITH (NOLOCK)
				INNER JOIN Nop_ProductVariant pv2 ON pv2.ProductVariantId = opv2.ProductVariantId
				INNER JOIN Nop_Product p2 ON p2.ProductId = pv2.ProductId			
			WHERE 
				p2.ProductID = @ProductID
		)
		AND 
			(
				p.ProductId != @ProductID
			)
		AND 
			(
				@ShowHidden = 1 OR p.Published = 1
			)
		AND 
			(
				p.Deleted = 0
			)
		AND 
			(
				@ShowHidden = 1
				OR
				GETUTCDATE() BETWEEN ISNULL(pv.AvailableStartDateTime, '1/1/1900') AND ISNULL(pv.AvailableEndDateTime, '1/1/2999')
			)
	GROUP BY
		p.ProductId
	ORDER BY 
		ProductsPurchased desc


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
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
		p.[UpdatedOn]
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Product p on p.ProductID = [pi].ProductID
		LEFT OUTER JOIN Nop_ProductLocalized pl with (NOLOCK) ON p.ProductID = pl.ProductID AND pl.LanguageID = @LanguageID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex

END
