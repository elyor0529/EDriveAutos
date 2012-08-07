-- =============================================
-- Author:		Bhavesh Tarkhala
-- Create date: 12-5-2011 12:00 PM
-- Description:	To get category names comma separated
-- =============================================
--GetCategoryNamesByPortfolio 5
CREATE FUNCTION [dbo].[GetStateProvinceID]
	-- Add the parameters for the stored procedure here
(
	@Abbreviation varchar(MAX)
)
RETURNS int
BEGIN

    -- Insert statements for procedure here
	Declare @StateNo int
	SELECT @StateNo = StateProvinceID 
	FROM Nop_StateProvince 
	WHERE Abbreviation =@Abbreviation
		
	RETURN @StateNo
END