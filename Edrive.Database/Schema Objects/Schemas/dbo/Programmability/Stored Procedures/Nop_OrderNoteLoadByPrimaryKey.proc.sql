

CREATE PROCEDURE [dbo].[Nop_OrderNoteLoadByPrimaryKey]
(
	@OrderNoteID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderNote]
	WHERE
		OrderNoteID = @OrderNoteID
END
