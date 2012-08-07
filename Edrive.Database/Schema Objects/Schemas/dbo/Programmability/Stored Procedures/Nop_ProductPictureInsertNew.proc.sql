-- EXEC [dbo].[Nop_ProductPictureInsertNew] null,null,'http://cdn.getauto.com/photos/5/23785/1c/2B3LJ44V89H535708-1c.jpg;http://cdn.getauto.com/photos/5/23785/2c/2B3LJ44V89H535708-2c.jpg;http://cdn.getauto.com/photos/5/23785/3c/2B3LJ44V89H535708-3c.jpg;http://cdn.getauto.com/photos/5/23785/4c/2B3LJ44V89H535708-4c.jpg;http://cdn.getauto.com/photos/5/23785/5c/2B3LJ44V89H535708-5c.jpg;http://cdn.getauto.com/photos/5/23785/6c/2B3LJ44V89H535708-6c.jpg;http://cdn.getauto.com/photos/5/23785/7c/2B3LJ44V89H535708-7c.jpg;http://cdn.getauto.com/photos/5/23785/8c/2B3LJ44V89H535708-8c.jpg;http://cdn.getauto.com/photos/5/23785/9c/2B3LJ44V89H535708-9c.jpg;http://cdn.getauto.com/photos/5/23785/10c/2B3LJ44V89H535708-10c.jpg;http://cdn.getauto.com/photos/5/23785/11c/2B3LJ44V89H535708-11c.jpg;http://cdn.getauto.com/photos/5/23785/12c/2B3LJ44V89H535708-12c.jpg;http://cdn.getauto.com/photos/5/23785/13c/2B3LJ44V89H535708-13c.jpg;http://cdn.getauto.com/photos/5/23785/14c/2B3LJ44V89H535708-14c.jpg;http://cdn.getauto.com/photos/5/23785/15c/2B3LJ44V89H535708-15c.jpg;http://cdn.getauto.com/photos/5/23785/16c/2B3LJ44V89H535708-16c.jpg;http://cdn.getauto.com/photos/5/23785/17c/2B3LJ44V89H535708-17c.jpg;http://cdn.getauto.com/photos/5/23785/18c/2B3LJ44V89H535708-18c.jpg;http://cdn.getauto.com/photos/5/23785/19c/2B3LJ44V89H535708-19c.jpg;http://cdn.getauto.com/photos/5/23785/20c/2B3LJ44V89H535708-20c.jpg'
CREATE PROCEDURE [dbo].[Nop_ProductPictureInsertNew] 
(
    @ProductPictureID int = NULL output,
    @VIN nvarchar(MAX),
    @Pics nvarchar(MAX)
  )
AS
BEGIN
    
    Declare @ProductID as int
    select @ProductID = ProductId from Nop_Product where VIN = @VIN
    
        
    If Object_Id('tempdb..#TempPicture') Is Not Null 
	Drop Table #TempPicture

	Create Table #TempPicture
	(
		Id Bigint IDENTITY(1,1),
		PictureList varchar(MAX)
	)

	Insert Into #TempPicture
	SELECT data FROM [NOP_splitstring_to_table](@Pics,';' )
    
    
    --select * from #TempPicture
    
    
    Declare @Count As Bigint
	Declare @cnt as bigint
	Declare @Temp as varchar(MAX)
	Set @cnt=0
	Select @Count=Max(Id) From #TempPicture

	While (Select @cnt)<@count
	Begin
		Set @cnt = @cnt + 1
		Select @Temp= PictureList from #TempPicture where Id=@cnt
		Set @Temp = LTrim(RTrim(@Temp))
		INSERT INTO [Nop_ProductPicture]
		(
			ProductId,PictureID,DisplayOrder,PictureURL
		)
		VALUES
		(
			@ProductID,0,0,@Temp
		)
	End
		
    
    --INSERT INTO [Nop_ProductPicture]
    --(
    --    ProductId,PictureID,DisplayOrder,PictureURL
    --)
    --VALUES
    --(
    --    @ProductID,0,0,''
    --)
	
    set @ProductPictureID=SCOPE_IDENTITY()
    
    If Object_Id('tempdb..#TempPicture') Is Not Null 
	Drop Table #TempPicture
END