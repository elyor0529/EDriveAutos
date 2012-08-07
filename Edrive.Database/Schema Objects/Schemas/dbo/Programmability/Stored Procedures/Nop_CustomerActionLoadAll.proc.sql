

CREATE PROCEDURE [dbo].[Nop_CustomerActionLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_CustomerAction]
	ORDER BY [DisplayOrder], [Name]
END
