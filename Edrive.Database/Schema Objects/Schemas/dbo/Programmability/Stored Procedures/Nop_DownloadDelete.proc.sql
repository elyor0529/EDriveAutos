

CREATE PROCEDURE [dbo].[Nop_DownloadDelete]
(
	@DownloadID int
)
AS
BEGIN
	SET NOCOUNT ON
	DELETE
	FROM [Nop_Download]
	WHERE
		DownloadID = @DownloadID
END
