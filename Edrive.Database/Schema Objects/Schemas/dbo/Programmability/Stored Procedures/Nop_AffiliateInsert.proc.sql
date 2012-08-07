

CREATE PROCEDURE [dbo].[Nop_AffiliateInsert]
(
	@AffiliateID int = NULL output,
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@MiddleName nvarchar(100),
	@PhoneNumber nvarchar(50),
	@Email nvarchar(255),
	@FaxNumber nvarchar(50),
	@Company nvarchar(100),
	@Address1 nvarchar(100),
	@Address2 nvarchar(100),
	@City nvarchar(100),
	@StateProvince nvarchar(100),
	@ZipPostalCode nvarchar(10),
	@CountryID int,
	@Deleted bit,
	@Active bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Affiliate]
	(
		FirstName,
		LastName,
		MiddleName,
		PhoneNumber,
		Email,
		FaxNumber,
		Company,
		Address1,
		Address2,
		City,
		StateProvince,
		ZipPostalCode,
		CountryID,
		Deleted,
		Active
	)
	VALUES
	(
		@FirstName,
		@LastName,
		@MiddleName,
		@PhoneNumber,
		@Email,
		@FaxNumber,
		@Company,
		@Address1,
		@Address2,
		@City,
		@StateProvince,
		@ZipPostalCode,
		@CountryID,
		@Deleted,
		@Active
	)

	set @AffiliateID=SCOPE_IDENTITY()
END
