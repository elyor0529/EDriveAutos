

CREATE PROCEDURE [dbo].[Nop_PricelistInsert]
(
	@ExportModeID int,
	@ExportTypeID int,
	@AffiliateID int,
	@DisplayName nvarchar(100),
	@ShortName nvarchar(20),
	@PricelistGuid nvarchar(40),
	@CacheTime int,
	@FormatLocalization nvarchar(5),
	@Description nvarchar(500),
	@AdminNotes nvarchar(500),
	@Header nvarchar(500),
	@Body nvarchar(500),
	@Footer nvarchar(500),
	@PriceAdjustmentTypeID int,
	@PriceAdjustment money,
	@OverrideIndivAdjustment bit,
	@CreatedOn datetime,
	@UpdatedOn datetime,
	@PricelistID int OUTPUT
)
AS
BEGIN
	INSERT 
	INTO [Nop_Pricelist] 
	(
		[ExportModeID],
		[ExportTypeID],
		[AffiliateID],
		[DisplayName],
		[ShortName],
		[PricelistGuid],
		[CacheTime],
		[FormatLocalization],
		[Description],
		[AdminNotes],
		[Header],
		[Body],
		[Footer],
		[PriceAdjustmentTypeID],
		[PriceAdjustment],
		[OverrideIndivAdjustment],
		[CreatedOn],
		[UpdatedOn]
	) 
	VALUES 
	(
		@ExportModeID,
		@ExportTypeID,
		@AffiliateID,
		@DisplayName,
		@ShortName,
		@PricelistGuid,
		@CacheTime,
		@FormatLocalization,
		@Description,
		@AdminNotes,
		@Header,
		@Body,
		@Footer,
		@PriceAdjustmentTypeID,
		@PriceAdjustment,
		@OverrideIndivAdjustment,
		@CreatedOn,
		@UpdatedOn
	)

	SET @PricelistID = SCOPE_IDENTITY()
END
