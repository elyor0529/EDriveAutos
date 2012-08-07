

CREATE PROCEDURE [dbo].[Nop_BannedIpNetworkUpdate]
	@BannedIpNetworkID int,
	@StartAddress nvarchar(50),
	@EndAddress nvarchar(50),
	@Comment nvarchar(500),
	@IpException nvarchar(max),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	UPDATE Nop_BannedIpNetwork
	 SET
		StartAddress = @StartAddress,
		EndAddress = @EndAddress,
		Comment = @Comment,
		IpException = @IpException,
		CreatedOn = @CreatedOn,
		UpdatedOn = @UpdatedOn
	WHERE 
		BannedIpNetworkID = @BannedIpNetworkID

END
