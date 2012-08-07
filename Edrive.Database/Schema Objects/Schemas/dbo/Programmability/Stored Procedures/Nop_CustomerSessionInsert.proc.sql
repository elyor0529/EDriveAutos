

CREATE PROCEDURE [dbo].[Nop_CustomerSessionInsert]
(
	@CustomerSessionGUID uniqueidentifier,
	@CustomerID int,
	@LastAccessed datetime,
	@IsExpired bit
)
AS
BEGIN
	INSERT
	INTO [Nop_CustomerSession]
	(
		CustomerSessionGUID,
		CustomerID,
		LastAccessed,
		IsExpired
	)
	VALUES
	(
		@CustomerSessionGUID,
		@CustomerID,
		@LastAccessed,
		@IsExpired
	)

END
