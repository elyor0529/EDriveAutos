--[dbo].[Nop_CustomerLoadAllNew] null,null,null,null,0,'Troy Brown',null,'Carl Steele'
--[dbo].[Nop_CustomerLoadAllNew] null,null,null,null,0,null,null,'Carl Steele'
--[dbo].[Nop_CustomerLoadAllNew] '9/12/2011 12:00:00 AM',null
CREATE PROCEDURE [dbo].[Nop_CustomerLoadAllNew] 
(
	@StartTime				datetime = NULL,
	@EndTime				datetime = NULL,
	@Email					nvarchar(200),
	@Username				nvarchar(200),
	@DontLoadGuestCustomers	bit = 0,
	@CompanyName			nvarchar(200),
	@LastName               nvarchar(200),
	@DealerName			nvarchar(200)
)
AS
BEGIN
	
	Declare @Sql as nvarchar(MAX)
	Declare @Paramlist as nvarchar(500)
	Declare @finalSql as nvarchar(max)
	Declare @Temp as varchar(5)
	select @Temp = '%'
	
	
	set @Sql = 'Select * From Nop_Customer AS c 
	
	Left Outer join 
	Nop_CustomerAttribute as ca
	On c.CustomerId = ca.CustomerId	and ca.[Key]=''Company''
	
	Left Outer join 
	Nop_CustomerAttribute as ca1
	On c.CustomerId = ca1.CustomerId and ca1.[Key]=''LastName''
	
	
	where c.IsGuest=0 And c.deleted=0 And c.CustomerType = 1
		
	'
				
	
	
	If(@StartTime != '')
		Begin
			set @Sql = @Sql + ' And c.RegistrationDate <= '''+Convert(varchar,@StartTime)+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
	
	
	If(@EndTime != '')
		Begin
			set @Sql = @Sql + ' And c.RegistrationDate >= '''+Convert(varchar,@EndTime)+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end
		
	If(@Email != '')
		Begin
			set @Sql = @Sql + ' And c.Email = '''+@Email+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end	
		
			
	If(@Username != '')
		Begin
			set @Sql = @Sql + ' And c.Username = '''+@Username+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end	
		
		
	If(@DontLoadGuestCustomers = 'FALSE')
	Begin
		set @Sql = @Sql + ' And c.[IsGuest] = 0 ' 
	End
	Else
	Begin
		set @Sql = @Sql + ' And c.[IsGuest] = 1 '
	end
	
	
	If(@CompanyName != '' And @DealerName !='')
		Begin
			set @Sql = @Sql + ' And ca.[Value] in ( '''+@CompanyName+''','''+@DealerName+''' )' 
		End
	Else if(@CompanyName!='')
		Begin
			set @Sql = @Sql + ' And ca.[Value] = '''+@CompanyName+''' ' 
		end	
	Else if(@DealerName!='')
	Begin
		set @Sql = @Sql + ' And ca.[Value] = '''+@DealerName+''' ' 
	end	
	Else
	Begin
		set @Sql =@Sql
	End
		
		
	
	If(@LastName != '')
		Begin
			set @Sql = @Sql + ' And ca1.[Value] = '''+@LastName+''' ' 
		End
	Else
		Begin
			set @Sql = @Sql
		end	
	
		
	print(@sql)
	set @Paramlist = '@StartTime datetime ,@EndTime datetime,@Email varchar,@Username varchar,@DontLoadGuestCustomers bit,@CompanyName varchar,@LastName varchar,@DealerName varchar'
	exec sp_executesql @Sql ,@Paramlist,@StartTime,@EndTime,@Email,@Username,@DontLoadGuestCustomers,@CompanyName,@LastName,@DealerName

	
END