

CREATE PROCEDURE [dbo].[Nop_PricelistUpdate]
(
	@PricelistID int,
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
	@UpdatedOn datetime
)
AS
BEGIN
	UPDATE [Nop_Pricelist] 
	SET
		[ExportModeID] = @ExportModeID,
		[ExportTypeID] = @ExportTypeID,
		[AffiliateID] = @AffiliateID,
		[DisplayName] = @DisplayName,
		[ShortName] = @ShortName,
		[PricelistGuid] = @PricelistGuid,
		[CacheTime] = @CacheTime,
		[FormatLocalization] = @FormatLocalization,
		[Description] = @Description,
		[AdminNotes] = @AdminNotes,
		[Header] = @Header,
		[Body] = @Body,
		[Footer] = @Footer,
		[PriceAdjustmentTypeID] = @PriceAdjustmentTypeID,
		[PriceAdjustment] = @PriceAdjustment,
		[OverrideIndivAdjustment] = @OverrideIndivAdjustment,
		[CreatedOn] = @CreatedOn,
		[UpdatedOn] = @UpdatedOn
	WHERE
		[PricelistID] = @PricelistID
END
