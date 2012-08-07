

CREATE PROCEDURE [dbo].[Nop_CustomerSessionUpdate]
(
	@CustomerSessionGUID uniqueidentifier,
	@CustomerID int,
	@LastAccessed datetime,
	@IsExpired bit
)
AS
BEGIN
	UPDATE [Nop_CustomerSession]
	SET
		CustomerID=@CustomerID,
		LastAccessed=@LastAccessed,
		IsExpired=@IsExpired
	WHERE
		[CustomerSessionGUID] = @CustomerSessionGUID
END
