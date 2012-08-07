

CREATE PROCEDURE [dbo].[Nop_ActivityLogLoadAll]
(
	@CreatedOnFrom datetime = NULL,
	@CreatedOnTo datetime = NULL,
	@Email nvarchar(200),
	@Username nvarchar(200),
	@ActivityLogTypeID int,
	@PageIndex int = 0, 
	@PageSize int = 2147483644,
	@TotalRecords int = null OUTPUT
)
AS
BEGIN
	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'


	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		ActivityLogID int NOT NULL,
		CreatedOn datetime NOT NULL
	)

	INSERT INTO #PageIndex (ActivityLogID, CreatedOn)
	SELECT DISTINCT
		al.ActivityLogID,
		al.CreatedOn
	FROM [Nop_ActivityLog] al with (NOLOCK)
	INNER JOIN [Nop_Customer] c on c.CustomerID = al.CustomerID
	WHERE 
		(@CreatedOnFrom is NULL or @CreatedOnFrom <= al.CreatedOn) and
		(@CreatedOnTo is NULL or @CreatedOnTo >= al.CreatedOn) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		(c.IsGuest=0) and (c.deleted=0) and
		(@ActivityLogTypeID is null or (al.ActivityLogTypeID=@ActivityLogTypeID)) 
	ORDER BY al.CreatedOn DESC

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT
		al.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_ActivityLog] al on al.ActivityLogID = [pi].ActivityLogID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex	
END
