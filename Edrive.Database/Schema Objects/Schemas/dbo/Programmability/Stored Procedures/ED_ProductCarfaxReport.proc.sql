-- =============================================
-- Author:		<Henisha Rathod>
-- Create date: <14/2/2011>
-- Description:	Bulk product insert
-- Updated Date: <29/7/2011>
-- For Product Specification Attribute Mapping section
-- =============================================

-- EXEC [dbo].[ED_ProductCarfaxReport] 'D:\Nyusoft\Development\EDrive\edriveauto_cfx_08192011_return_file.txt'
CREATE PROCEDURE [dbo].[ED_ProductCarfaxReport] --'D:\EDrive\CARFAX\edriveauto_cfx_09162011_return_file.txt'
	@File nvarchar(500)
AS
BEGIN

Declare @ParaList as nvarchar(500)
Declare @Sql as nvarchar(1000)
Select @Paralist = '@File nvarchar(MAX)' 

	/************ FOR PRODUCT ************/

	--Nirav - 15-Sep-11
	--Reset OwnerDetail field before updating records with new file
	Update Nop_Product Set OwnerDetail=NULL Where OwnerDetail Is Not NULL

	--If Object_Id('tempdb..#Temp') Is Not Null 
	--Drop Table #Temp

	--Create Table #Temp
	--(
	--	VIN nvarchar(max),
	--	DealerID nvarchar(max),
	--	ExpiryDate nvarchar(max),
	--	OwnerDetail nvarchar(max)
	--)

	Select @Sql = 'BULK INSERT ED_TempCarfax FROM "' + @File + '"
	WITH
	(
	  FIRSTROW = 2
	 ,FIELDTERMINATOR = ''|''
	 ,BATCHSIZE = 100
	)'

	Exec sp_executesql @Sql,@ParaList,@File
	
	Declare @cnt int
	set @cnt=0
	
	Begin Tran
		select @cnt=COUNT(*) from ED_TempCarfax
		
		if(@cnt!=0)
		Begin
		update p
		set
		p.OwnerDetail = t.OwnerDetail
		from Nop_Product as p
		right join
		ED_TempCarfax as t
		on p.VIN =t.VIN 
		End	
			
		Truncate Table ED_TempCarfax	
	Commit Tran
END