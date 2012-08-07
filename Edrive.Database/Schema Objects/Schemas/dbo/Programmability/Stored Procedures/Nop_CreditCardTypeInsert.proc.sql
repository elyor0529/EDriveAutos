

CREATE PROCEDURE [dbo].[Nop_CreditCardTypeInsert]
(
	@CreditCardTypeID int = NULL output,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@DisplayOrder int,
	@Deleted bit
)
AS
BEGIN
	INSERT
	INTO [Nop_CreditCardType]
	(
		[Name],
		SystemKeyword,
		DisplayOrder,
		Deleted
	)
	VALUES
	(
		@Name,
		@SystemKeyword,
		@DisplayOrder,
		@Deleted
	)

	set @CreditCardTypeID=SCOPE_IDENTITY()
END
