-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Insert DealersFAQ
-- =============================================

CREATE PROCEDURE [dbo].[ED_InterestedCustomerInsert]
(
	@InterestedCustomerID bigint = NULL output,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@ProductId int,
	@CustomerId int,
	@Email nvarchar(MAX),
	@PhoneNumber nvarchar(50),
	@ZipCode nvarchar(50),
	@ContactType int,
	@InterestType int,
	@Price_Lock bit,
	@Financing bit,
	@Trade_in bit,
	@AdditionalComment nvarchar(MAX),
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_InterestedCustomer]
	(
		[FirstName],
		[LastName],
		[ProductId],
		[CustomerId],
		[Email],
		[PhoneNumber],
		[ZipCode],
		[ContactType],
		[InterestType],
		[Price_Lock],
		[Financing],
		[Trade_in],
		[AdditionalComment], 
		[IsActive],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@FirstName,
		@LastName,
		@ProductId,
		@CustomerId,
		@Email,
		@PhoneNumber,
		@ZipCode,
		@ContactType,
		@InterestType,
		@Price_Lock,
		@Financing,
		@Trade_in,
		@AdditionalComment,
		@IsActive,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @InterestedCustomerID = SCOPE_IDENTITY()

	RETURN @Err
END