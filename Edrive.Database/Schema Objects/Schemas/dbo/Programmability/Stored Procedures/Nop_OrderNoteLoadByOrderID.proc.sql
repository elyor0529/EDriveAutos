

CREATE PROCEDURE [dbo].[Nop_OrderNoteLoadByOrderID]
(
	@OrderID int,
	@ShowHidden bit
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_OrderNote]
	WHERE
		OrderID=@OrderID AND
		(@ShowHidden = 1 OR DisplayToCustomer = 1)
	ORDER BY CreatedOn desc, OrderNoteID desc
END
