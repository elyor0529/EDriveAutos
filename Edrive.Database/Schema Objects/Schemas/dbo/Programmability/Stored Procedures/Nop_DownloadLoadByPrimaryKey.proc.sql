

CREATE PROCEDURE [dbo].[Nop_DownloadLoadByPrimaryKey]
(
	@DownloadID int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		*
	FROM [Nop_Download]
	WHERE
		DownloadID = @DownloadID
END
