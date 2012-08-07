

CREATE PROCEDURE [dbo].[Nop_BannedIpAddressDelete]
	@BannedIpAddressID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_BannedIpAddress 
	WHERE BannedIpAddressID = @BannedIpAddressID
	
END
