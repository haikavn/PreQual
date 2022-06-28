-- =============================================
-- Author:		<Author,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetHistory]
	@dateFrom datetime,
	@dateTo datetime,
	@Action int = 0,
	@UserId bigint = 0,
	@UserIds varchar(MAX),
	@Entity varchar(50) = '',
	@EntityId bigint = 0,
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
	AND ((@UserId = 0 OR History.[UserID] = @UserId) or (len(@UserIds) > 0 and History.[UserID] in (select Item from dbo.SplitInts(@UserIds, ','))))
	AND (@EntityId = 0 OR History.[EntityID] = @EntityId)
	AND (len(@Entity) = 0 or History.[Entity] = @Entity)
	ORDER BY History.Id Desc
	OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;

END