-- =============================================
-- Author:		<Author,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHistoryByUsers]
	@dateFrom datetime,
	@dateTo datetime,
	@Action int = 0,
	@UserIds varchar(MAX),
	@start int,
	@count int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT  Id, [Date], Module, [Action], Entity, EntityID, Data1, Data2, Note, UserID
	FROM dbo.History
	WHERE History.[Date] BETWEEN @dateFrom AND @dateTo
	AND (@Action = 0 OR History.[Action] = @Action)
	AND (len(@UserIds) = 0 or (len(@UserIds) > 0 and History.[UserID] in (select Item from dbo.SplitInts(@UserIds, ','))))
	ORDER BY History.Id Desc
	OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;

END