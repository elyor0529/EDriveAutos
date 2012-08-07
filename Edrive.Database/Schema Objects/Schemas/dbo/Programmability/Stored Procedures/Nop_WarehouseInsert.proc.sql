

CREATE PROCEDURE [dbo].[Nop_WarehouseInsert]
(
	@WarehouseID int = NULL output,
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
	INSERT
	INTO [Nop_Warehouse]
	(
		[Name],
		PhoneNumber,
		Email,
		FaxNumber,
		Address1,
		Address2,
		City,
		StateProvince,
		ZipPostalCode,
		CountryID,
		Deleted,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@PhoneNumber,
		@Email,
		@FaxNumber,
		@Address1,
		@Address2,
		@City,
		@StateProvince,
		@ZipPostalCode,
		@CountryID,
		@Deleted,
		@CreatedOn,
		@UpdatedOn
	)

	set @WarehouseID=SCOPE_IDENTITY()
END
