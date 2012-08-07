-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <21/2/2011>
-- Description:	Get uploaded file name for junk data
-- =============================================
CREATE PROCEDURE [dbo].[ED_GetFileNameForJunkProductData] --37
	@CustomerID int
AS
BEGIN
	Select Distinct [FileName]
	From
	ED_JunkProductData
	Where 1=1
	And CustomerID = @CustomerID And [FileName] is not null
END










