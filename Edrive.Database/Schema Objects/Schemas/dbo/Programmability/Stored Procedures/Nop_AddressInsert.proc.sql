

CREATE PROCEDURE [dbo].[Nop_AddressInsert]
(
	@AddressId int = NULL output,
	@CustomerID int,
	@IsBillingAddress bit,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@PhoneNumber nvarchar(50),
	@Email nvarchar(255),
	@FaxNumber nvarchar(50),
	@Company nvarchar(100),
	@Address1 nvarchar(100),
	@Address2 nvarchar(100),
	@City nvarchar(100),
	@StateProvinceID int,
	@ZipPostalCode nvarchar(10),
	@CountryID int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN
	INSERT
	INTO [Nop_Address]
	(
		CustomerID,
		IsBillingAddress,
		FirstName,
		LastName,
		PhoneNumber,
		Email,
		FaxNumber,
		Company,
		Address1,
		Address2,
		City,
		StateProvinceID,
		ZipPostalCode,
		CountryID,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@CustomerID,
		@IsBillingAddress,
		@FirstName,
		@LastName,
		@PhoneNumber,
		@Email,
		@FaxNumber,
		@Company,
		@Address1,
		@Address2,
		@City,
		@StateProvinceID,
		@ZipPostalCode,
		@CountryID,
		@CreatedOn,
		@UpdatedOn
	)

	set @AddressId=SCOPE_IDENTITY()
END
