-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Insert DealersFAQ
-- =============================================

Create PROCEDURE [dbo].[ED_DealersFaqInsert]
(
	@FaqId bigint = NULL output,
	@OrderId bigint,
	@Question varchar(MAX),
	@Answer nvarchar(MAX),
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_DealersFaq]
	(
		[OrderId],
		[Question],
		[Answer],
		[IsActive],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@OrderId,
		@Question,
		@Answer,
		@IsActive,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @FaqId = SCOPE_IDENTITY()

	RETURN @Err
END