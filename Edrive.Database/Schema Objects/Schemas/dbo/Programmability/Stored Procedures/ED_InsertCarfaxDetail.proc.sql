CREATE PROCEDURE [dbo].[ED_InsertCarfaxDetail]
(
	@LogMsg nvarchar(500),
	@Status int,
	@Success int,
	@CreateBy int,
	@CreateOn datetime
)
AS
BEGIN
	INSERT
	INTO [ED_CarfaxLogDetail]
	(
		[LogMsg], 
		[Status] ,
		[Success] ,
		[CreateBy] ,
		[CreateOn] 
	)
	VALUES
	(
		@LogMsg,
		@Status,
		@Success,
		@CreateBy,
		@CreateOn
	)

	
END