

CREATE PROCEDURE [dbo].[Nop_NewsLoadByPrimaryKey]
(
	@NewsID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_News]
	WHERE
		NewsID=@NewsID
END
