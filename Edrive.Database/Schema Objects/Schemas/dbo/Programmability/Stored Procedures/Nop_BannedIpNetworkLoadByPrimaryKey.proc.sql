

CREATE PROCEDURE [dbo].[Nop_BannedIpNetworkLoadByPrimaryKey]
	@BannedIpNetworkID int
AS
BEGIN
	SET NOCOUNT ON

	SELECT * 
	FROM Nop_BannedIpNetwork
	WHERE BannedIpNetworkID = @BannedIpNetworkID
	ORDER BY BannedIpNetworkID

END
