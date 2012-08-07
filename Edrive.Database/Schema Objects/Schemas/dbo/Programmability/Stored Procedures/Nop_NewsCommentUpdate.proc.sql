

CREATE PROCEDURE [dbo].[Nop_NewsCommentUpdate]
(
	@NewsCommentID int,
	@NewsID int,
	@CustomerID int,
	@IPAddress nvarchar(100),
	@Title nvarchar(1000),
	@Comment nvarchar(max),
	@CreatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_NewsComment]
	SET
		NewsID=@NewsID,
		CustomerID=@CustomerID,
		IPAddress=@IPAddress,
		Title=@Title,
		Comment=@Comment,
		CreatedOn=@CreatedOn
	WHERE
		NewsCommentID = @NewsCommentID
END
