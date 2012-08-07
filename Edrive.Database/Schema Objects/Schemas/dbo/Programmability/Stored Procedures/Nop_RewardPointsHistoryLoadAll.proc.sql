

CREATE PROCEDURE [dbo].[Nop_RewardPointsHistoryLoadAll]
(
	@CustomerID int,
	@OrderID int,
	@PageIndex int = 0, 
	@PageSize int = 2147483644,
	@TotalRecords int = null OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		RewardPointsHistoryID int NOT NULL,
		CreatedOn datetime NOT NULL
	)

	INSERT INTO #PageIndex (RewardPointsHistoryID, CreatedOn)
	SELECT DISTINCT
		rph.RewardPointsHistoryID,
		rph.CreatedOn
	FROM [Nop_RewardPointsHistory] rph with (NOLOCK)
	WHERE 
		(
			@CustomerID IS NULL OR @CustomerID=0
			OR (rph.CustomerID=@CustomerID)
		)
		AND
		(
			@OrderID IS NULL OR @OrderID=0
			OR (rph.OrderID=@OrderID)
		)
	ORDER BY rph.CreatedOn DESC, RewardPointsHistoryID

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT
		rph.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_RewardPointsHistory] rph on rph.RewardPointsHistoryID = [pi].RewardPointsHistoryID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
END
