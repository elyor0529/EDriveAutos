
-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Update FAQ
-- =============================================

CREATE PROCEDURE [dbo].[ED_FaqUpdate]
(
	@FaqId bigint,	
	@Question varchar(MAX),
	@Answer nvarchar(MAX),
	@UpdatedOn datetime = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [ED_Faq]
	SET		
		[Question] = @Question,
		[Answer] = @Answer,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[FaqId] = @FaqId


	SET @Err = @@Error


	RETURN @Err
END



