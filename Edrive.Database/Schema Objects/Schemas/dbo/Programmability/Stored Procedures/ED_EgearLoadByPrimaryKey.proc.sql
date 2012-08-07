-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <29/3/2011>
-- Description:	Get Edrive Product by id
-- =============================================

Create PROCEDURE [dbo].[ED_EgearLoadByPrimaryKey]
(
	@EgearId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT *		
	FROM [ED_EGear] 
	WHERE
		(eGearID = @EgearId)
	and deleted = 0
	
	SET @Err = @@Error

	RETURN @Err
END