-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Get Testimonial by id
-- =============================================

CREATE PROCEDURE [dbo].[ED_TestimonialsLoadByPrimaryKey]
(
	@TId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[TId],
		[TContent],
		[Name],
		[Address],
		[PictureId],
		[IsActive],
		[CreatedOn],
		[UpdatedOn]
	FROM [ED_Testimonials]
	WHERE
		([TId] = @TId)

	SET @Err = @@Error

	RETURN @Err
END

