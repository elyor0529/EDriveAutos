

CREATE PROCEDURE [dbo].[Nop_OrderNoteUpdate]
(
	@OrderNoteID int,
	@OrderID int,
	@Note nvarchar(4000),
	@DisplayToCustomer bit,
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_OrderNote]
	SET
	OrderID=@OrderID,
	Note=@Note,
	DisplayToCustomer = @DisplayToCustomer,
	CreatedOn=@CreatedOn
	WHERE
		OrderNoteID = @OrderNoteID
END
