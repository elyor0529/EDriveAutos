

CREATE PROCEDURE [dbo].[Nop_CurrencyInsert]
(
	@CurrencyID int = NULL output,
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
	INSERT
	INTO [Nop_Currency]
	(
		[Name],
		CurrencyCode,
		Rate,
		DisplayLocale,
		CustomFormatting,
		Published,
		DisplayOrder,
		CreatedOn,
		UpdatedOn
	)
	VALUES
	(
		@Name,
		@CurrencyCode,
		@Rate,
		@DisplayLocale,
		@CustomFormatting,
		@Published,
		@DisplayOrder,
		@CreatedOn,
		@UpdatedOn
	)

	set @CurrencyID=SCOPE_IDENTITY()
END
