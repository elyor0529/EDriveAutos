CREATE PROCEDURE [dbo].[ImportFile]
@File VARCHAR(100)
AS

If Object_Id('tempdb..#Temp') Is Not Null 
	Drop Table #Temp

	Create Table #Temp
	(
		[1] nvarchar(max),
		[2] nvarchar(max),
		[3] nvarchar(max),
		[4] nvarchar(max),
		[5] nvarchar(max),
		[6] nvarchar(max),
		[7] nvarchar(max),
		[8] nvarchar(max),
		[9] nvarchar(max),
		[10] nvarchar(max),
		[11] nvarchar(max),
		[12] nvarchar(max),
		[13] nvarchar(max),
		[14] nvarchar(max),
		[15] nvarchar(max),
		[16] nvarchar(max),
		[17] nvarchar(max),
		[18] nvarchar(max),
		[19] nvarchar(max),
		[20] nvarchar(max),
		[21] nvarchar(max),
		[22] nvarchar(max),
		[23] nvarchar(max),
		[24] nvarchar(max),
		[25] nvarchar(max),
		[26] nvarchar(max),
		[27] nvarchar(max),
		[28] nvarchar(max)
	)


--Insert into #Temp([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],
--				[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],
--				[28])
--	SELECT
--	*
--	FROM
--	OPENROWSET('MSDASQL','Driver={Microsoft Text Driver (*.txt; *.csv)};DefaultDir=D:\Nyusoft\Development\EDrive','SELECT * FROM edriveautos.CSV')

-- One method to insert CSV data in SQL table


EXECUTE ('BULK INSERT #Temp FROM "' + @File + '"
WITH
(
FIRSTROW=2,
FIELDTERMINATOR = '',''
, ROWTERMINATOR = ''\n''
, CODEPAGE = ''RAW''
)' )

EXEC dbo.ImportFile 'D:\Nyusoft\Development\EDrive\edriveautos.csv'
select * from #Temp