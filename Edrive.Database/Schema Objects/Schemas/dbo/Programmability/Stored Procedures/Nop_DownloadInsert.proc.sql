

CREATE PROCEDURE [dbo].[Nop_DownloadInsert]
(
	@DownloadID int = NULL output,
	@UseDownloadURL bit,
	@DownloadURL nvarchar(400),
	@DownloadBinary varbinary(MAX),
	@ContentType nvarchar(20),
	@Filename nvarchar(100),
	@Extension nvarchar(20),
	@IsNew	bit
)
AS
BEGIN
	INSERT
	INTO [Nop_Download]
	(
		[UseDownloadURL],
		[DownloadURL],
		[DownloadBinary],
		[Filename],
		[ContentType],
		[Extension],
		[IsNew]
	)
	VALUES
	(
		@UseDownloadURL,
		@DownloadURL,
		@DownloadBinary,
		@Filename,
		@ContentType,
		@Extension,
		@IsNew
	)

	set @DownloadID=SCOPE_IDENTITY()
END
