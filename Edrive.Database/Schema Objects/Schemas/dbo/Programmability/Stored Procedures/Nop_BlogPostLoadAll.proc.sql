

CREATE PROCEDURE [dbo].[Nop_BlogPostLoadAll]
(
	@LanguageID	int,
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
		BlogPostID int NOT NULL,
	)

	INSERT INTO #PageIndex (BlogPostID)
	SELECT
		bp.BlogPostID
	FROM 
	    Nop_BlogPost bp 
	WITH 
		(NOLOCK)
	WHERE
		@LanguageID IS NULL OR @LanguageID = 0 OR LanguageID = @LanguageID
	ORDER BY 
		CreatedOn 
	DESC


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		bp.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_BlogPost bp on bp.BlogPostID = [pi].BlogPostID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
