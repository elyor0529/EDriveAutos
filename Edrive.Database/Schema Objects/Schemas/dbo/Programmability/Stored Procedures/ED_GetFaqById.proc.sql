-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <3/2/2011>
-- Description:	Get FAQ by id
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetFaqById]
(
	@FaqId bigint
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT * 
	FROM [ED_Faq]
	WHERE
		([FaqId] = @FaqId)

	SET @Err = @@Error

	RETURN @Err
END

