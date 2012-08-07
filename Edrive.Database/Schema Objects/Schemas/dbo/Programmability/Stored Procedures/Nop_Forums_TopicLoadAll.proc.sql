

CREATE PROCEDURE [dbo].[Nop_Forums_TopicLoadAll]
(
	@ForumID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),	
	@SearchPosts		bit = 0,
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@TotalRecords		int = null OUTPUT
)
AS
BEGIN
	
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = '%' + rtrim(ltrim(@Keywords)) + '%'

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
		TopicID int NOT NULL,
		TopicTypeID int NOT NULL,
		LastPostTime datetime NULL,
	)

	INSERT INTO #PageIndex (TopicID, TopicTypeID, LastPostTime)
	SELECT DISTINCT
		ft.TopicID, ft.TopicTypeID, ft.LastPostTime
	FROM Nop_Forums_Topic ft with (NOLOCK) 
	LEFT OUTER JOIN Nop_Forums_Post fp with (NOLOCK) ON ft.TopicID = fp.TopicID
	WHERE  (
				@ForumID IS NULL OR @ForumID=0
				OR (ft.ForumID=@ForumID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (ft.UserID=@UserID)
			)
		AND	(
				(patindex(@Keywords, isnull(ft.Subject, '')) > 0)
				or (@SearchPosts = 1 and patindex(@Keywords, isnull(fp.Text, '')) > 0)
			)		
	ORDER BY ft.TopicTypeID desc, ft.LastPostTime desc, ft.TopicID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		ft.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Topic ft on ft.TopicID = [pi].TopicID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
