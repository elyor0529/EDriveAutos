

CREATE PROCEDURE [dbo].[Nop_PollLoadAll]
	@LanguageID	int,
	@PollCount int,
	@ShowHidden bit
AS
BEGIN
	
	if (@PollCount > 0)
	      SET ROWCOUNT @PollCount

	SELECT *
	FROM [Nop_Poll]
	where (Published = 1 or @ShowHidden = 1)
	and (@LanguageID IS NULL or @LanguageID=0 or LanguageID = @LanguageID)
	order by DisplayOrder

	SET ROWCOUNT 0
END
