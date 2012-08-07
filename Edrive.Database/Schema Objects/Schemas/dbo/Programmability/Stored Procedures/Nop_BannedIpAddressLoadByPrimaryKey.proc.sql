

CREATE PROCEDURE [dbo].[Nop_BannedIpAddressLoadByPrimaryKey]
	@BannedIpAddressID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM Nop_BannedIpAddress
	WHERE BannedIpAddressID = @BannedIpAddressID
	ORDER BY BannedIpAddressID
	
END
