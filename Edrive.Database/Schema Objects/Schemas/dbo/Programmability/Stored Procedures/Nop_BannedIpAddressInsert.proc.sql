

CREATE PROCEDURE [dbo].[Nop_BannedIpAddressInsert]
	@BannedIpAddressID int = NULL output,
	@Address nvarchar(50),
	@Comment nvarchar(500),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	INSERT 
	INTO Nop_BannedIpAddress 
	(
		[Address],
		[Comment], 
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES 
	(
		@Address,
		@Comment,
		@CreatedOn,
		@UpdatedOn
	)
	
	SET @BannedIpAddressID = SCOPE_IDENTITY()

END
