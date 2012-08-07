--[dbo].[Nop_CustomerLoadAll] null,null,null,null,0,0,7000,0,1,'Troy Brown',null,null

CREATE PROCEDURE [dbo].[Nop_CustomerLoadAll]
(
	@StartTime				datetime = NULL,
	@EndTime				datetime = NULL,
	@Email					nvarchar(200),
	@Username				nvarchar(200),
	@DontLoadGuestCustomers	bit = 0,
	@PageIndex				int = 0, 
	@PageSize				int = 2147483644,
	@TotalRecords			int = null OUTPUT,
	@CustomerType			int,
	@CompanyName			nvarchar(200),
	@LastName               nvarchar(200),
	@DealerName			nvarchar(200)
)
AS
BEGIN

	SET @Email = isnull(@Email, '')
	SET @Email = '%' + rtrim(ltrim(@Email)) + '%'

	SET @Username = isnull(@Username, '')
	SET @Username = '%' + rtrim(ltrim(@Username)) + '%'

	SET @CompanyName = isnull(@CompanyName, '')
	SET @CompanyName = '%' + rtrim(ltrim(@CompanyName)) + '%'
	
	
	SET @DealerName = isnull(@DealerName, '')
	SET @DealerName = '%' + rtrim(ltrim(@DealerName)) + '%'
	
	SET @LastName = isnull(@LastName, '')
	SET @LastName = '%' + rtrim(ltrim(@LastName)) + '%'
	
	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	DECLARE @TotalThreads int
	
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1
	
	CREATE TABLE #PageIndex 
	(
		IndexID int IDENTITY (1, 1) NOT NULL,
		CustomerID int NOT NULL,
		RegistrationDate datetime NOT NULL,
	)

	INSERT INTO #PageIndex (CustomerID, RegistrationDate)
	SELECT DISTINCT
		c.CustomerID, c.RegistrationDate
	FROM [Nop_Customer] c with (NOLOCK)
	Left Join [Nop_CustomerAttribute] ca 
	ON c.CustomerID = ca.CustomerId and ca.[Key]='Company'
	
	Left Join [Nop_CustomerAttribute] ca1 
	ON c.CustomerID = ca1.CustomerId and ca1.[Key]='LastName'
	
	Left Join [Nop_CustomerAttribute] ca2 
	ON c.CustomerID = ca2.CustomerId and ca2.[Key]='Company'
	
	WHERE 
		(@StartTime is NULL or @StartTime <= c.RegistrationDate) and
		(@EndTime is NULL or @EndTime >= c.RegistrationDate) and 
		(patindex(@Email, isnull(c.Email, '')) > 0) and
		(patindex(@Username, isnull(c.Username, '')) > 0) and
		(patindex(@CompanyName, isnull(ca.[Value], '')) > 0) and 
		(patindex(@LastName, isnull(ca1.[Value], '')) > 0) and 
		(patindex(@DealerName, isnull(ca2.[Value], '')) > 0) and 
		 --ca.[Key]='Company') and
		(@DontLoadGuestCustomers = 0 or (c.IsGuest=0)) and 
		c.deleted=0 and CustomerType=@CustomerType
	order by c.RegistrationDate desc 

	SET @TotalRecords = @@rowcount	
	SET ROWCOUNT @RowsToReturn
	
	SELECT  
		c.*
	FROM
		#PageIndex [pi]
		INNER JOIN [Nop_Customer] c on c.CustomerID = [pi].CustomerID
	WHERE
		[pi].IndexID > @PageLowerBound AND 
		[pi].IndexID < @PageUpperBound
	ORDER BY
		IndexID
	
	SET ROWCOUNT 0

	DROP TABLE #PageIndex
	
END