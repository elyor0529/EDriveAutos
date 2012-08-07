-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <4/2/2011>
-- Description:	Update picture of testimonial
-- =============================================
CREATE PROCEDURE [dbo].[ED_UpdateTestimonialPicture]
	@TId int,
	@PictureId int
	
AS
BEGIN
	SET NOCOUNT ON;

    Update ED_Testimonials set PictureId=@PictureId Where IsActive=1
	And TId=@TId
END


