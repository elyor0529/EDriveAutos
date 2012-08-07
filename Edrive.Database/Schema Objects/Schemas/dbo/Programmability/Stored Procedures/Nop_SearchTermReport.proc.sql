

CREATE PROCEDURE [dbo].[Nop_SearchTermReport]
	@StartTime datetime = NULL,
	@EndTime datetime = NULL,
	@Count int
AS
BEGIN
	
	if (@Count > 0)
	      SET ROWCOUNT @Count

	SELECT SearchTerm, COUNT(1) as SearchCount FROM Nop_SearchLog
	WHERE
			(@StartTime is NULL or DATEDIFF(day, @StartTime, CreatedOn) >= 0) and
			(@EndTime is NULL or DATEDIFF(day, @EndTime, CreatedOn) <= 0) 
	GROUP BY SearchTerm 
	ORDER BY SearchCount desc

	SET ROWCOUNT 0
END
