

CREATE PROCEDURE [dbo].[Nop_OrderNoteInsert]
(
	@OrderNoteID int = NULL output,
	@OrderID int,
	@Note nvarchar(4000),
	@DisplayToCustomer bit,
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_OrderNote]
	(
		OrderID,
		Note,
		DisplayToCustomer,
		CreatedOn
	)
	VALUES
	(
		@OrderID,
		@Note,
		@DisplayToCustomer,
		@CreatedOn
	)

	set @OrderNoteID=SCOPE_IDENTITY()
END
