

CREATE PROCEDURE [dbo].[Nop_BannedIpAddressLoadAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM Nop_BannedIpAddress
	ORDER BY BannedIpAddressID
	
END
