-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Update DealersFAQ
-- =============================================

Create PROCEDURE [dbo].[ED_DealersFaqUpdate]
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

	UPDATE [ED_DealersFaq]
	SET		
		[Question] = @Question,
		[Answer] = @Answer,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[FaqId] = @FaqId


	SET @Err = @@Error


	RETURN @Err
END