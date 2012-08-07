

CREATE PROCEDURE [dbo].[Nop_NewsDelete]
(
	@NewsID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_News]
	WHERE
		[NewsID] = @NewsID
END
