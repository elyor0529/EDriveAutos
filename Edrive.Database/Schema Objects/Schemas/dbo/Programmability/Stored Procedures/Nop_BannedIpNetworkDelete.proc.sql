

CREATE PROCEDURE [dbo].[Nop_BannedIpNetworkDelete]
	@BannedIpNetworkID int
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM Nop_BannedIpNetwork
	WHERE BannedIpNetworkID = @BannedIpNetworkID

END
