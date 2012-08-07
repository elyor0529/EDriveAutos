-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <12/3/2011>
-- Description:	Get Product Detail from VIN
-- =============================================

CREATE PROCEDURE [dbo].[ED_GetProductDetailByVIN] --'33594444'
	@VIN varchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;

    Select ProductID,VIN
		--,MakeAttribute,BodyAttribute,MileageAttribute,
		--YearAttribute,PriceAttribute		 
	From Nop_Product Where VIN=@VIN
	And Deleted=0
END