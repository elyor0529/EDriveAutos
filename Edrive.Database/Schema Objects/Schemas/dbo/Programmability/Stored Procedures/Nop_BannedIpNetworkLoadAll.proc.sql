

CREATE PROCEDURE [dbo].[Nop_BannedIpNetworkLoadAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM Nop_BannedIpNetwork 
	ORDER BY BannedIpNetworkID

END
