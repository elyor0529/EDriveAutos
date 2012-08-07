
-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Insert FAQ
-- =============================================

CREATE PROCEDURE [dbo].[ED_FaqInsert]
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
	INTO [ED_Faq]
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

