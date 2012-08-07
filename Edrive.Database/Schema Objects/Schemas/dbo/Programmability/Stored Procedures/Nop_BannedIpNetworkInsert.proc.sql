

CREATE PROCEDURE [dbo].[Nop_BannedIpNetworkInsert]
	@BannedIpNetworkID int = NULL output,
	@StartAddress nvarchar(50),
	@EndAddress nvarchar(50),
	@Comment nvarchar(500),
	@IpException nvarchar(max),
	@CreatedOn datetime,
	@UpdatedOn datetime
AS
BEGIN

	INSERT 
	INTO Nop_BannedIpNetwork 
	(
		StartAddress,
		EndAddress,
		Comment,
		IpException,
		CreatedOn,
		UpdatedOn
	)
	VALUES 
	(
		@StartAddress,
		@EndAddress,
		@Comment,
		@IpException,
		@CreatedOn,
		@UpdatedOn
	)
	
	SET @BannedIpNetworkID = SCOPE_IDENTITY()
	
END
