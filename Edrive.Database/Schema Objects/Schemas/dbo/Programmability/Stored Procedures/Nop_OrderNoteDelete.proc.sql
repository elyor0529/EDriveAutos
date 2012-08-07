

CREATE PROCEDURE [dbo].[Nop_OrderNoteDelete]
(
	@OrderNoteID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_OrderNote]
	WHERE
		OrderNoteID = @OrderNoteID
END
