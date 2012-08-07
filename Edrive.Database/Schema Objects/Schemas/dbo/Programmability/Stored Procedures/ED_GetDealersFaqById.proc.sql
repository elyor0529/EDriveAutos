-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <6/5/2011>
-- Description:	Get Dealers FAQ by id
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetDealersFaqById]
(
	@FaqId bigint
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT * 
	FROM [ED_DealersFaq]
	WHERE
		([FaqId] = @FaqId)

	SET @Err = @@Error

	RETURN @Err
END