

CREATE PROCEDURE [dbo].[Nop_PictureDelete]
(
	@PictureID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Picture]
	WHERE
		PictureID = @PictureID
END
