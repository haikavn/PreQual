-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAffiliatesStatusCounts]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CAST([Status] AS int ) AS [Status], CAST(Count([Status]) AS int) AS Counts
	FROM [dbo].[Affiliate]
	WHERE Deleted is null or Deleted != 1
	GROUP BY [Status]


END