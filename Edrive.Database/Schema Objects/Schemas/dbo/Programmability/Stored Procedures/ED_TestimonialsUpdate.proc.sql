-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Update Testimonials
-- =============================================

CREATE PROCEDURE [dbo].[ED_TestimonialsUpdate]
(
	@TId int,
	@TContent varchar(MAX),
	@Name varchar(100),
	@Address varchar(MAX),
	@PictureId int,
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_Testimonials]
	SET
		[TContent] = @TContent,
		[Name] = @Name,
		[Address] = @Address,
		[PictureId] = @PictureId,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[TId] = @TId


	SET @Err = @@Error


	RETURN @Err
END


