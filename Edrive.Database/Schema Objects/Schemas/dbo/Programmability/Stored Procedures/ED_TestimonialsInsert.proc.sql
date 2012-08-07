-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Insert Testimonials
-- =============================================

CREATE PROCEDURE [dbo].[ED_TestimonialsInsert]
(
	@TId bigint = NULL output,
	@TContent varchar(MAX),
	@Name varchar(100),
	@Address varchar(MAX),
	@PictureId int,
	@IsActive bit,
	@CreatedOn datetime,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT
	INTO [ED_Testimonials]
	(
		[TContent],
		[Name],
		[Address],
		[PictureId],
		[IsActive],
		[CreatedOn],
		[UpdatedOn]
	)
	VALUES
	(
		@TContent,
		@Name,
		@Address,
		@PictureId,
		@IsActive,
		@CreatedOn,
		@UpdatedOn
	)

	SET @Err = @@Error

	SELECT @TId = SCOPE_IDENTITY()

	RETURN @Err
END

