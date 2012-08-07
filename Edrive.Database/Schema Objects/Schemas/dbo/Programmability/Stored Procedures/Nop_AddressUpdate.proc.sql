

CREATE PROCEDURE [dbo].[Nop_AddressUpdate]
(
	@AddressId int,
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
	UPDATE [Nop_Address]
	SET
	CustomerID=@CustomerID,
	IsBillingAddress=@IsBillingAddress,
	FirstName=@FirstName,
	LastName=@LastName,
	PhoneNumber=@PhoneNumber,
	Email=@Email,
	FaxNumber=@FaxNumber,
	Company=@Company,
	Address1=@Address1,
	Address2=@Address2,
	City=@City,
	StateProvinceID=@StateProvinceID,
	ZipPostalCode=@ZipPostalCode,
	CountryID=@CountryID,
	CreatedOn=@CreatedOn,
	UpdatedOn=@UpdatedOn
	WHERE
		[AddressId] = @AddressId
END
