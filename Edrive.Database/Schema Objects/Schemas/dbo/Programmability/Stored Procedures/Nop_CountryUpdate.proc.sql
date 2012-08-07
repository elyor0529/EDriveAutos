

CREATE PROCEDURE [dbo].[Nop_CountryUpdate]
(
	@CountryId int,
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
	UPDATE [Nop_Country]
	SET
		[Name] = @Name,
		[AllowsRegistration]=@AllowsRegistration,		
		[AllowsBilling]=@AllowsBilling,
		[AllowsShipping] =@AllowsShipping,
		[TwoLetterISOCode] = @TwoLetterISOCode,
		[ThreeLetterISOCode] = @ThreeLetterISOCode,
		[NumericISOCode] = @NumericISOCode,
		[Published] = @Published,
		[DisplayOrder] = @DisplayOrder
	WHERE
		[CountryId] = @CountryId
END
