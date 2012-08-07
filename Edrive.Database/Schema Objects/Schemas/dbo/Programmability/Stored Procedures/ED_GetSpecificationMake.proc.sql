-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetSpecificationMake] 
	@Make nvarchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [SpecificationAttributeOptionID]
      ,[SpecificationAttributeID]
      ,[Name]
      ,[DisplayOrder]
      ,[AttributeOptionFrom]
      ,[AttributeOptionTo]
  FROM [Nop_SpecificationAttributeOption]
  where [Name] = @Make

END