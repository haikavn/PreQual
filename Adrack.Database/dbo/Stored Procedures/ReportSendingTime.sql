-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[ReportSendingTime] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@campaigns VARCHAR(MAX)
AS
BEGIN
	IF OBJECT_ID('[TempDB].[dbo].[#CampaignIds]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#CampaignIds]
		END

	CREATE TABLE [dbo].[#CampaignIds]
	(	
		[Id] BIGINT NULL,
	)

	--------- Insert Parsed Buyers Channel Id ---------
	INSERT INTO [dbo].[#CampaignIds] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@campaigns, ',');

	select
		'Before' as TimeoutType,
		'Sold' as StatusType,
		c.Id,
		c.Name, 
		min(r.ResponseTime) / 1000 as MinSec, 
		avg(r.ResponseTime) / 1000 as AvgSec, 
		max(r.ResponseTime) / 1000 as MaxSec, 
		count(distinct r.leadid) as Quantity 
	from LeadMainResponse r
	inner join Buyer c on r.BuyerId = c.Id
	where r.[Status] = 1 and r.ErrorType != 17 and r.ErrorType != 18 and r.Created between @start and @end and r.CampaignId in (select Id from [#CampaignIds])
	group by c.[Name], c.Id
	union
	select
		'Before' as TimeoutType,
		'Reject' as StatusType,
		c.Id,
		c.Name, 
		min(r.ResponseTime) / 1000 as MinSec, 
		avg(r.ResponseTime) / 1000 as AvgSec, 
		max(r.ResponseTime) / 1000 as MaxSec, 
		count(distinct r.leadid) as Quantity 
	from LeadMainResponse r
	inner join Buyer c on r.BuyerId = c.Id
	where r.[Status] = 3 and r.ErrorType != 17 and r.ErrorType != 18 and r.Created between @start and @end and r.CampaignId in (select Id from [#CampaignIds])
	group by c.[Name], c.Id
	union
	select
		'After' as TimeoutType,
		'Sold' as StatusType,
		c.Id,
		c.Name, 
		min(r.ResponseTime) / 1000 as MinSec, 
		avg(r.ResponseTime) / 1000 as AvgSec, 
		max(r.ResponseTime) / 1000 as MaxSec, 
		count(distinct r.leadid) as Quantity 
	from LeadMainResponse r
	inner join Buyer c on r.BuyerId = c.Id
	where r.[Status] = 1 and (r.ErrorType = 17 or r.ErrorType = 18 or Response = 'Channel timeout') and r.Created between @start and @end and r.CampaignId in (select Id from [#CampaignIds])
	group by c.[Name], c.Id
	union
	select
		'After' as TimeoutType,
		'Reject' as StatusType,
		c.Id,
		c.Name, 
		min(r.ResponseTime) / 1000 as MinSec, 
		avg(r.ResponseTime) / 1000 as AvgSec, 
		max(r.ResponseTime) / 1000 as MaxSec, 
		count(distinct r.leadid) as Quantity 
	from LeadMainResponse r
	inner join Buyer c on r.BuyerId = c.Id
	where r.[Status] = 3 and (r.ErrorType = 17 or r.ErrorType = 18 or Response = 'Channel timeout') and r.Created between @start and @end and r.CampaignId in (select Id from [#CampaignIds])
	group by c.[Name], c.Id
	union
	select
		'Paused' as TimeoutType,
		'Reject' as StatusType,
		c.Id,
		c.Name, 
		min(r.ResponseTime) / 1000 as MinSec, 
		avg(r.ResponseTime) / 1000 as AvgSec, 
		max(r.ResponseTime) / 1000 as MaxSec, 
		count(distinct r.leadid) as Quantity 
	from LeadMainResponse r
	inner join Buyer c on r.BuyerId = c.Id
	where r.[Status] = 3 and (r.ErrorType = 18) and r.Created between @start and @end and r.CampaignId in (select Id from [#CampaignIds])
	group by c.[Name], c.Id
END