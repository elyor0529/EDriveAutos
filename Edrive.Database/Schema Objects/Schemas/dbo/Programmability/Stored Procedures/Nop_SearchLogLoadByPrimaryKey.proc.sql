

CREATE PROCEDURE [dbo].[Nop_SearchLogLoadByPrimaryKey]
(
	@SearchLogID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_SearchLog]
	WHERE
		SearchLogID = @SearchLogID
END
