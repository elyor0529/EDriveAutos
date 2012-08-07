

CREATE PROCEDURE [dbo].[Nop_CurrencyUpdate]
(
	@CurrencyID int,
	@Name nvarchar(50),
	@CurrencyCode nvarchar(5),
	@Rate decimal (18, 4),
	@DisplayLocale nvarchar(50),
	@CustomFormatting nvarchar(50),
	@Published bit,
	@DisplayOrder int,
	@CreatedOn datetime,
	@UpdatedOn datetime
)
AS
BEGIN

	UPDATE [Nop_Currency]
	SET
		[Name]=@Name,
		CurrencyCode=@CurrencyCode,
		Rate=@Rate,
		DisplayLocale=@DisplayLocale,
		CustomFormatting=@CustomFormatting,
		Published=@Published,
		DisplayOrder=@DisplayOrder,
		CreatedOn=@CreatedOn,
		UpdatedOn=@UpdatedOn
	WHERE
		CurrencyID = @CurrencyID

END
