

CREATE PROCEDURE [dbo].[Nop_WarehouseUpdate]
(
	@WarehouseID int,
	@Name nvarchar(255),
	@PhoneNumber nvarchar(50),
	@Email nvarchar(255),
	@FaxNumber nvarchar(50),
	@Address1 nvarchar(100),
	@Address2 nvarchar(100),
	@City nvarchar(100),
	@StateProvince nvarchar(100),
	@ZipPostalCode nvarchar(10),
	@CountryID int,
	@Deleted bit,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Warehouse]
	SET
	[Name]=@Name,
	PhoneNumber=@PhoneNumber,
	Email=@Email,
	FaxNumber=@FaxNumber,
	Address1=@Address1,
	Address2=@Address2,
	City=@City,
	StateProvince=@StateProvince,
	ZipPostalCode=@ZipPostalCode,
	CountryID=@CountryID,
	Deleted=@Deleted,
	CreatedOn=@CreatedOn,
	UpdatedOn=@UpdatedOn
	WHERE
		[WarehouseID] = @WarehouseID
END
