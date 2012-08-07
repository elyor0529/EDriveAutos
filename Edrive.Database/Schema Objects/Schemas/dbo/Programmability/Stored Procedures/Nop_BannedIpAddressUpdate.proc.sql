

CREATE PROCEDURE [dbo].[Nop_BannedIpAddressUpdate]
	@BannedIpAddressID int,
	@Address nvarchar(50),
	@Comment nvarchar(500),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	UPDATE Nop_BannedIpAddress 
	SET
		[Address] = @Address,
		[Comment] = @Comment,
		[CreatedOn] = @CreatedOn,
		[UpdatedOn] = @UpdatedOn
	WHERE 
		BannedIpAddressID = @BannedIpAddressID

END
