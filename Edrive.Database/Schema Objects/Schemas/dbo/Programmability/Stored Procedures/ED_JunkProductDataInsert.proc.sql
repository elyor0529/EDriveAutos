-- =============================================
-- Author:		<Manali Panchal>
-- Create date: <18/2/2011>
-- Description:	Insert Junk Product Data
-- =============================================
CREATE PROCEDURE [dbo].[ED_JunkProductDataInsert]
(
	@VIN varchar(100),
	@CustomerID int,
	@VehicleName varchar(MAX),
	@Type varchar(MAX),
	@Stock varchar(MAX),
	@Year varchar(MAX),
	@Make varchar(MAX),
	@Model varchar(MAX),
	@Trim varchar(MAX),
	@Free_Text varchar(MAX),
	@Body varchar(MAX),
	@Mileage varchar(MAX),
	@Price_Current money,
	@Reserved varchar(MAX),
	@Price_Wholesale money,
	@Price_Cost money,
	@Title varchar(MAX),
	@Condition varchar(MAX),
	@Exterior_Color varchar(MAX),
	@Interior_Color varchar(MAX),
	@Doors varchar(MAX),
	@Engine varchar(MAX),
	@Transmission varchar(MAX),
	@Fuel_Type varchar(MAX),
	@Drive_Type varchar(MAX),
	@Options varchar(MAX),
	@Warranty varchar(MAX),
	@Description varchar(MAX),
	@Pics varchar(MAX),
	@DealerName varchar(MAX),
	@DealerZip varchar(MAX),
	@Date_in_Stock varchar(MAX),
	@City varchar(MAX),
	@StateID int,
	@CreatedOn datetime,
	@ParaId int,
	@FileName varchar(MAX)
)
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	
	INSERT
	INTO [ED_JunkProductData]
	(
		VIN,
		CustomerID,
		VehicleName,
		[Type],
		Stock,
		[Year],
		Make,
		Model,
		Trim,
		Free_Text,
		Body,
		Mileage,
		Price_Current,
		Reserved,
		Price_Wholesale,
		Price_Cost,
		Title,
		Condition,
		Exterior_Color,
		Interior_Color,
		Doors,
		Engine,
		Transmission,
		Fuel_Type,
		Drive_Type,
		Options,
		Warranty,
		Description,
		Pics,
		Dealer_Name,
		Dealer_Zip,
		City,
		StateID,
		Date_in_Stock,
		CreatedOn,
		ParaId,
		[FileName]
	)
	VALUES
	(
		@VIN,
		@CustomerID,
		@VehicleName,
		@Type,
		@Stock,
		@Year,
		@Make,
		@Model,
		@Trim,
		@Free_Text,
		@Body,
		@Mileage,
		@Price_Current,
		@Reserved,
		@Price_Wholesale,
		@Price_Cost,
		@Title,
		@Condition,
		@Exterior_Color,
		@Interior_Color,
		@Doors,
		@Engine,
		@Transmission,
		@Fuel_Type,
		@Drive_Type,
		@Options,
		@Warranty,
		@Description,
		@Pics,
		@DealerName,
		@DealerZip,
		@City,
		@StateID,
		@Date_in_Stock,
		@CreatedOn,
		@ParaId,
		@FileName
	)

	SET @Err = @@Error	
	
	
	RETURN @Err
END