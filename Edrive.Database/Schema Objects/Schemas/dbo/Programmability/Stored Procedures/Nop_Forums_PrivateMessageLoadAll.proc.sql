

CREATE PROCEDURE [dbo].[Nop_Forums_PrivateMessageLoadAll]
(
	@FromUserID			int,
	@ToUserID			int,
	@IsRead				bit = null,	--0 not read only, 1 read only, null - load all messages
	@IsDeletedByAuthor		bit = null,	--0 deleted by author only, 1 not deleted by author only, null - load all messages
	@IsDeletedByRecipient	bit = null,	--0 deleted by recipient only, 1 not deleted by recipient, null - load all messages
	@Keywords			nvarchar(MAX),
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
		PrivateMessageID int NOT NULL,
		CreatedOn datetime NOT NULL,
	)

	INSERT INTO #PageIndex (PrivateMessageID, CreatedOn)
	SELECT DISTINCT
		fpm.PrivateMessageID, fpm.CreatedOn
	FROM Nop_Forums_PrivateMessage fpm with (NOLOCK)
	WHERE   (
				@FromUserID IS NULL OR @FromUserID=0
				OR (fpm.FromUserID=@FromUserID)
			)
		AND (
				@ToUserID IS NULL OR @ToUserID=0
				OR (fpm.ToUserID=@ToUserID)
			)
		AND (
				@IsRead IS NULL OR fpm.IsRead=@IsRead
			)
		AND (
				@IsDeletedByAuthor IS NULL OR fpm.IsDeletedByAuthor=@IsDeletedByAuthor
			)
		AND (
				@IsDeletedByRecipient IS NULL OR fpm.IsDeletedByRecipient=@IsDeletedByRecipient
			)
		AND	(
				(patindex(@Keywords, isnull(fpm.Subject, '')) > 0)
				or (patindex(@Keywords, isnull(fpm.Text, '')) > 0)
			)
	ORDER BY fpm.CreatedOn desc, fpm.PrivateMessageID desc

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		fpm.*
	FROM
		#PageIndex [pi]
		INNER JOIN Nop_Forums_PrivateMessage fpm on fpm.PrivateMessageID = [pi].PrivateMessageID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0
END
