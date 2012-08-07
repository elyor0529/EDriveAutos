

CREATE PROCEDURE [dbo].[Nop_Forums_SubscriptionLoadAll]
(
	@UserID				int,
	@ForumID			int,
	@TopicID			int,
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
		SubscriptionID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (SubscriptionID, CreatedOn)
	SELECT DISTINCT
		fs.SubscriptionID, fs.CreatedOn
	FROM Nop_Forums_Subscription fs with (NOLOCK)
	INNER JOIN Nop_Customer c with (NOLOCK) ON fs.UserID=c.CustomerID
	WHERE   (
				@UserID IS NULL OR @UserID=0
				OR (fs.UserID=@UserID)
			)
		AND (
				@ForumID IS NULL OR @ForumID=0
				OR (fs.ForumID=@ForumID)
			)
		AND (
				@TopicID IS NULL OR @TopicID=0
				OR (fs.TopicID=@TopicID)
			)
		AND (
				c.Active=1 AND c.Deleted=0
			)
	ORDER BY fs.CreatedOn desc, fs.SubscriptionID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fs.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Subscription fs on fs.SubscriptionID = [pi].SubscriptionID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
