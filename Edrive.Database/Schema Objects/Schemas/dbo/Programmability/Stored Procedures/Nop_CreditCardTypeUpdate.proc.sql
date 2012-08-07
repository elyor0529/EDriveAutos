

CREATE PROCEDURE [dbo].[Nop_CreditCardTypeUpdate]
(
	@CreditCardTypeID int,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@DisplayOrder int,
	@Deleted bit
)
AS
BEGIN
	UPDATE [Nop_CreditCardType]
	SET
		[Name]=@Name,
		SystemKeyword=@SystemKeyword,
		DisplayOrder=@DisplayOrder,
		Deleted=@Deleted
	WHERE
		CreditCardTypeID = @CreditCardTypeID
END
