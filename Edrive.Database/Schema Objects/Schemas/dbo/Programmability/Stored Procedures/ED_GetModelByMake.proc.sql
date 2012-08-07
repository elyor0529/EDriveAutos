-- ================================================
-- Author:		<Manali Panchal>
-- Create date: <25/3/2011>
-- Description:	For getting Specification Attribute Options for Dropdown
-- ================================================

--Exec [dbo].[ED_GetModelByMake] 'All'
CREATE PROCEDURE [dbo].[ED_GetModelByMake] --'All'
	@Make varchar(50)

AS
BEGIN

SET NOCOUNT ON;
	Select Distinct(Model)
	From Nop_Product
	Where Make=@Make	
	SET NOCOUNT ON;
	
	--UNION 
	--Select 'Select Model','0' FROM Nop_Product
	--ORDER BY value
END