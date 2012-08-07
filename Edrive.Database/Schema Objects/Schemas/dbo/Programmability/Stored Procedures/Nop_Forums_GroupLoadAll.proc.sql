

CREATE PROCEDURE [dbo].[Nop_Forums_GroupLoadAll]
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [Nop_Forums_Group]
	order by DisplayOrder
END
