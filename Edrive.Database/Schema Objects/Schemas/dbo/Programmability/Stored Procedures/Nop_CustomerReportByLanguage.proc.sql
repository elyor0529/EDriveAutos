

CREATE PROCEDURE [dbo].[Nop_CustomerReportByLanguage]
AS
BEGIN

	SELECT c.LanguageId, COUNT(c.LanguageId) as CustomerCount
	FROM [Nop_Customer] c
	WHERE
		c.Deleted = 0
	GROUP BY c.LanguageId
	ORDER BY CustomerCount desc

END
