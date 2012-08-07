

CREATE PROCEDURE [dbo].[Nop_PictureLoadByPrimaryKey]
(
	@PictureID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Picture]
	WHERE
		PictureID = @PictureID
END
