

CREATE PROCEDURE [dbo].[Nop_MeasureWeightInsert]
(
	@MeasureWeightID int = NULL output,
	@Name nvarchar(100),
	@SystemKeyword nvarchar(100),
	@Ratio decimal(18, 4),
	@DisplayOrder int
)
AS
BEGIN
	INSERT
	INTO [Nop_MeasureWeight]
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

	set @MeasureWeightID=SCOPE_IDENTITY()
END
