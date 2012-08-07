

CREATE PROCEDURE [dbo].[Nop_CustomerSessionLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CustomerSession]
	order by LastAccessed desc
END
