

CREATE PROCEDURE [dbo].[Nop_MeasureDimensionInsert]
(
	@MeasureDimensionID int = NULL output,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Ratio decimal(18, 4),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_MeasureDimension]
	(
		[Name],
		[SystemKeyword],
		[Ratio],
		[DisplayOrder]
	)
	VALUES
	(
		@Name,
		@SystemKeyword,
		@Ratio,
		@DisplayOrder
	)

	set @MeasureDimensionID=SCOPE_IDENTITY()
END
