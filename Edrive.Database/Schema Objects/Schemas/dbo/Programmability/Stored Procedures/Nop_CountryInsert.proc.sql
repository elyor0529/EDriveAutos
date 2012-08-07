

CREATE PROCEDURE [dbo].[Nop_CountryInsert]
(
	@CountryId int = NULL output,
	@Name nvarchar(100),
	@AllowsRegistration bit, 
	@AllowsBilling bit, 
	@AllowsShipping bit, 
	@TwoLetterISOCode nvarchar(2),
	@ThreeLetterISOCode nvarchar(3),
	@NumericISOCode int,
	@Published bit,
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_Country]
	(
		[Name],
		[AllowsRegistration],
		[AllowsBilling],
		[AllowsShipping],
		[TwoLetterISOCode],
		[ThreeLetterISOCode],
		[NumericISOCode],
		[Published],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@AllowsRegistration,
		@AllowsBilling,
		@AllowsShipping,
		@TwoLetterISOCode,
		@ThreeLetterISOCode,
		@NumericISOCode,
		@Published,
		@DisplayOrder
	)

	set @CountryId=SCOPE_IDENTITY()
END
