-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ED_InsertDataImportStatus] --'137FA90372E197683','Test2_37_09112011111522.csv',2
	-- Add the parameters for the stored procedure here
	@VIN Varchar(50),
	@FileName Varchar(50),
	@Status Int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @Desc Varchar(1000)
	
	if(@Status=1)
		Begin
			Set @Desc='Junk Record';
		End
	else if(@Status=2)
		Begin
			Set @Desc='Not a 4-Wheeler Record';
		End
	else if(@Status=3)
		Begin
			Set @Desc='New Record Inserted';
		End
	else if(@Status=4)
		Begin
			Set @Desc='Record Updated';
		End
	else if(@Status=5)
		Begin
			Set @Desc='NADA Validation Failed';
		End		
	else if(@Status=6)
		Begin
			Set @Desc='NADA Webservice Error';
		End		
		
	Insert Into ED_DataImportLog
	(VIN,[FileName],[Status],[Description],CreatedDate)
	Values
	(@VIN,@FileName,@Status,@Desc,GETDATE())
END