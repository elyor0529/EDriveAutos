

CREATE PROCEDURE [dbo].[Nop_NewsCommentInsert]
(
	@NewsCommentID int = NULL output,
	@NewsID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@Comment nvarchar(max),
	@CreatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_NewsComment]
	(
		NewsID,
		CustomerID,
		IPAddress,
		Title,
		Comment,
		CreatedOn
	)
	VALUES
	(
		@NewsID,
		@CustomerID,
		@IPAddress,
		@Title,
		@Comment,
		@CreatedOn
	)

	set @NewsCommentID=SCOPE_IDENTITY()
END
