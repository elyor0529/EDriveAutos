

CREATE PROCEDURE [dbo].[Nop_Forums_PostLoadAll]
(
	@TopicID			int,
	@UserID				int,
	@Keywords			nvarchar(MAX),
	@AscSort			bit = 1,
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
		PostID int NOT NULL,
	)

	INSERT INTO #PageIndex (PostID)
	SELECT
		fp.PostID
	FROM Nop_Forums_Post fp with (NOLOCK)
	WHERE   (
				@TopicID IS NULL OR @TopicID=0
				OR (fp.TopicID=@TopicID)
			)
		AND (
				@UserID IS NULL OR @UserID=0
				OR (fp.UserID=@UserID)
			)
		AND	(
				patindex(@Keywords, isnull(fp.Text, '')) > 0
			)
	ORDER BY
		CASE @AscSort WHEN 0 THEN fp.CreatedOn END DESC,
		CASE @AscSort WHEN 1 THEN fp.CreatedOn END,
		fp.PostID


	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fp.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_Post fp on fp.PostID = [pi].PostID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
