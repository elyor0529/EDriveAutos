

CREATE PROCEDURE [dbo].[Nop_AffiliateUpdate]
(
	@AffiliateID int,
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

	UPDATE [Nop_Affiliate]
	SET
		FirstName=@FirstName,
		LastName=@LastName,
		MiddleName=@MiddleName,
		PhoneNumber=@PhoneNumber,
		Email=@Email,
		FaxNumber=@FaxNumber,
		Company=@Company,
		Address1=@Address1,
		Address2=@Address2,
		City=@City,
		StateProvince=@StateProvince,
		ZipPostalCode=@ZipPostalCode,
		CountryID=@CountryID,
		Deleted=@Deleted,
		Active=@Active
	WHERE
		AffiliateID = @AffiliateID

END
