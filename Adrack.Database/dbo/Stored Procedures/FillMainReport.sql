-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FillMainReport]
	-- Add the parameters for the stored procedure here
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- drop table leadresponses_temp

	if (dbo.CheckStoredProcActive('FillMainReport')> 0)
	begin
		select -1;
	    return;
	end;

	declare @TimeZone int = 0;

	select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

	if @TimeZone is null 
	begin
		set @TimeZone = 0;
	end;

	
    -- Insert statements for procedure here
	declare @maxid bigint;
	declare @id bigint;
	---declare @minid bigint;

	select @id = ISNULL(max(ResponseId), 0) from LeadMainReport;
	select @maxid = ISNULL(max(Id), 0) from LeadMainResponse;

	declare @leadid bigint;
	declare @maxleadid bigint;

	select @leadid = ISNULL(max(LeadId), 0) from LeadMainReportDayReceived;
	select @maxleadid = ISNULL(max(Id), 0) from LeadMain;

	if @id is null set @id = 0;
	if @maxid is null set @maxid = 0;
	--if @minid is null set @minid = 0;

	print @id;
	print @maxid;

	WAITFOR DELAY '00:00:02:000';
	--begin tran;

	select Id, Created, LeadId, CampaignId, BuyerId, AffiliateId, BuyerChannelId, AffiliateChannelId, [Status], AffiliatePrice, BuyerPrice, [State], ResponseTime, CampaignType
	into #leadresponses_temp
	from LeadMainResponse
	where Id > @id and Id <= @maxid and Status != 7 and Status != 5 and Status != 0;

	--BEGIN TRAN

	insert into LeadMainReport(ResponseId, Created, LeadId, CampaignId, BuyerId, AffiliateId, BuyerChannelId, AffiliateChannelId, [Status], AffiliatePrice, BuyerPrice, [State], ResponseTime, CampaignType)
	select Id, Created, LeadId, CampaignId, BuyerId, AffiliateId, BuyerChannelId, AffiliateChannelId, [Status], AffiliatePrice, BuyerPrice, [State], ResponseTime, CampaignType
	from #leadresponses_temp

	--WAITFOR DELAY '00:00:01:000';

	merge LeadMainReportDay t
	using (SELECT distinct [CampaignId]
		  ,[CampaignType]
		  ,[BuyerId]
		  ,[AffiliateId]
		  ,[AffiliateChannelId]
		  ,[BuyerChannelId]
		  ,[Status]
		  ,[State]
		  ,sum([AffiliatePrice]) as AffiliatePrice
		  ,sum([BuyerPrice]) as BuyerPrice
		  ,cast(dateadd(Hour, @TimeZone, Created) as date) as Created
		  ,count(LeadId) as Quantity
	  FROM #leadresponses_temp-- where id > @minid
	  group by 
		   [Campaignid]
		  ,[Campaigntype]
		  ,[BuyerId] 
		  ,[AffiliateId]
		  ,[AffiliateChannelId]
		  ,[BuyerChannelId]
		  ,[Status]
		  ,[State]
		  ,cast(dateadd(Hour, @TimeZone, Created) as date)) s
		on (t.Created = s.Created and t.CampaignId = s.CampaignId and t.AffiliateId = s.AffiliateId and t.BuyerId = s.BuyerId and t.AffiliateChannelId = s.AffiliateChannelId and t.BuyerChannelId = s.BuyerChannelId and t.Status = s.Status and t.State = s.State)
			WHEN NOT MATCHED BY TARGET
			THEN INSERT(CampaignId, CampaignType, BuyerId, AffiliateId, AffiliateChannelId, BuyerChannelId, Status, AffiliatePrice, BuyerPrice, Created, Quantity, State) VALUES(s.CampaignId, s.CampaignType, s.BuyerId, s.AffiliateId, s.AffiliateChannelId, s.BuyerChannelId, s.Status, s.AffiliatePrice, s.BuyerPrice, s.Created, s.Quantity, s.State)
			WHEN MATCHED 
			THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity, t.AffiliatePrice = t.AffiliatePrice + s.AffiliatePrice, t.BuyerPrice = t.BuyerPrice + s.BuyerPrice;

	merge LeadMainReportDayHour t
	using (SELECT distinct [CampaignId]
		  ,[Campaigntype]
		  ,[BuyerId]
		  ,[AffiliateId]
		  ,[AffiliateChannelId]
		  ,[BuyerChannelId]
		  ,[Status]
		  ,[State]
		  ,sum([AffiliatePrice]) as AffiliatePrice
		  ,sum([BuyerPrice]) as BuyerPrice
		  ,cast(dateadd(Hour, @TimeZone, Created) as date) as Created
		  ,DATEPART(Hour, dateadd(Hour, @TimeZone, Created)) as [Hour]
		  ,count(LeadId) as Quantity
	  FROM #leadresponses_temp-- where id > @minid
	  group by 
		   [Campaignid]
		  ,[Campaigntype]
		  ,[BuyerId] 
		  ,[AffiliateId]
		  ,[AffiliateChannelId]
		  ,[BuyerChannelId]
		  ,[Status]
		  ,[State]
		  ,cast(dateadd(Hour, @TimeZone, Created) as date)
		  ,DATEPART(Hour, dateadd(Hour, @TimeZone, Created))
		  ) s
		on (t.Created = s.Created and t.[Hour] = s.[Hour] and t.CampaignId = s.CampaignId and t.AffiliateId = s.AffiliateId and t.BuyerId = s.BuyerId and t.AffiliateChannelId = s.AffiliateChannelId and t.BuyerChannelId = s.BuyerChannelId and t.Status = s.Status and t.State = s.State)
			WHEN NOT MATCHED BY TARGET
			THEN INSERT(CampaignId, CampaignType, BuyerId, AffiliateId, AffiliateChannelId, BuyerChannelId, Status, AffiliatePrice, BuyerPrice, Created, [Hour], Quantity, State) VALUES(s.CampaignId, s.CampaignType, s.BuyerId, s.AffiliateId, s.AffiliateChannelId, s.BuyerChannelId, s.Status, s.AffiliatePrice, s.BuyerPrice, s.Created, s.[Hour], s.Quantity, s.State)
			WHEN MATCHED 
			THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity, t.AffiliatePrice = t.AffiliatePrice + s.AffiliatePrice, t.BuyerPrice = t.BuyerPrice + s.BuyerPrice;

		merge LeadMainReportDayAffiliate t
		using (SELECT distinct [CampaignId]
			  ,leadmain.[CampaignType]
			  ,leadmain.[AffiliateId]
			  ,AffiliateChannelId
			  ,[State]
			  ,cast(dateadd(Hour, @TimeZone, leadmain.Created) as date) as Created
			  ,count(leadmain.id) as Quantity
		  FROM LeadMain inner join leadcontent on leadcontent.leadid = leadmain.id where leadmain.Id > @leadid and leadmain.Id <= @maxleadid
		  group by
			[CampaignId]
			  ,leadmain.[CampaignType]
			  ,leadmain.[AffiliateId]
			  ,leadmain.AffiliateChannelId
			  ,[State]
			  ,cast(dateadd(Hour, @TimeZone, leadmain.Created) as date)
		  ) s
			on (t.Created = s.Created and t.CampaignId = s.CampaignId and t.AffiliateId = s.AffiliateId and t.AffiliateChannelId = s.AffiliateChannelId and t.State = s.State)
				WHEN NOT MATCHED BY TARGET
				THEN INSERT(CampaignId, CampaignType, AffiliateId, AffiliateChannelId, Status, Created, Quantity, State, LeadId) VALUES(s.CampaignId, s.CampaignType, s.AffiliateId, s.AffiliateChannelId, 0, s.Created, s.Quantity, s.State, @maxleadid)
				WHEN MATCHED 
				THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity, t.LeadId = @maxleadid;

		merge LeadMainReportDayReceived t
		using (SELECT distinct [CampaignId]
			  ,[CampaignType]
			  ,[AffiliateId]
			  ,AffiliateChannelId
			  ,cast(dateadd(Hour, @TimeZone, Created) as date) as Created
			  ,count(Id) as Quantity
		  FROM LeadMain where Id > @leadid and Id <= @maxleadid
		  group by
			[CampaignId]
			  ,[CampaignType]
			  ,[AffiliateId]
			  ,AffiliateChannelId
			  ,cast(dateadd(Hour, @TimeZone, Created) as date)
		  ) s
			on (t.Created = s.Created and t.CampaignId = s.CampaignId and t.AffiliateId = s.AffiliateId and t.AffiliateChannelId = s.AffiliateChannelId)
				WHEN NOT MATCHED BY TARGET
				THEN INSERT(CampaignId, CampaignType, AffiliateId, AffiliateChannelId, Created, Quantity, LeadId) VALUES(s.CampaignId, s.CampaignType, s.AffiliateId, s.AffiliateChannelId, s.Created, s.Quantity, @maxleadid)
				WHEN MATCHED 
				THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity, t.LeadId = @maxleadid;

		merge LeadMainReportDayRedirected t
		using (SELECT distinct tmp.[CampaignId]
			  ,tmp.[CampaignType]
			  ,tmp.[AffiliateId]
			  ,tmp.AffiliateChannelId
			  ,tmp.BuyerId
			  ,tmp.BuyerChannelId
			  ,tmp.State
			  ,cast(dateadd(Hour, @TimeZone, tmp.Created) as date) as Created
			  ,count(distinct tmp.LeadId) as Quantity
		  FROM #leadresponses_temp tmp inner join RedirectUrl r on tmp.LeadId = r.LeadId and r.Clicked = 1 where tmp.Status = 1
		  group by
			tmp.[CampaignId]
			  ,tmp.[CampaignType]
			  ,tmp.[AffiliateId]
			  ,tmp.AffiliateChannelId
			  ,tmp.BuyerId
			  ,tmp.BuyerChannelId
			  ,tmp.State
			  ,cast(dateadd(Hour, @TimeZone, tmp.Created) as date)
		  ) s
			on (t.Created = s.Created and t.CampaignId = s.CampaignId and t.AffiliateId = s.AffiliateId and t.AffiliateChannelId = s.AffiliateChannelId and t.BuyerId = s.BuyerId and t.BuyerChannelId = s.BuyerChannelId and t.State = s.State)
				WHEN NOT MATCHED BY TARGET
				THEN INSERT(CampaignId, CampaignType, AffiliateId, AffiliateChannelId, BuyerId, BuyerChannelId, Created, Quantity, [State]) VALUES(s.CampaignId, s.CampaignType, s.AffiliateId, s.AffiliateChannelId, s.BuyerId, s.BuyerChannelId, s.Created, s.Quantity, s.State)
				WHEN MATCHED 
				THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity;

			merge LeadMainReportDayPrices t
			using (SELECT distinct [CampaignId]
				  ,[CampaignType]
				  ,[BuyerId]
				  ,[BuyerChannelId]
				  ,[Status]
				  ,BuyerPrice
				  ,cast(dateadd(Hour, @TimeZone, Created) as date) as Created
				  ,count(LeadId) as Quantity
				  ,count(distinct LeadId) as UQuantity
			  FROM #leadresponses_temp-- where id > @minid
			  group by 
				   [CampaignId]
				  ,[CampaignType]
				  ,[BuyerId]
				  ,[BuyerChannelId]
				  ,[Status]
				  ,BuyerPrice
				  ,cast(dateadd(Hour, @TimeZone, Created) as date)) s
				on (t.Created = s.Created and t.CampaignId = s.CampaignId and t.BuyerId = s.BuyerId and t.BuyerChannelId = s.BuyerChannelId and t.BuyerPrice = s.BuyerPrice and t.[Status] = s.[Status])
					WHEN NOT MATCHED BY TARGET
					THEN INSERT(CampaignId, CampaignType, BuyerId, BuyerChannelId, BuyerPrice, Created, Quantity, UQuantity, [Status]) VALUES(s.CampaignId, s.CampaignType, s.BuyerId, s.BuyerChannelId, s.BuyerPrice, s.Created, s.Quantity, s.UQuantity, s.[Status])
					WHEN MATCHED 
					THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity, t.UQuantity = t.UQuantity + s.UQuantity;


		merge LeadMainReportDaySubIds t
		using (SELECT distinct 
			  lr.[Status]
			  ,lc.[AffiliateSubId]
			  ,cast(dateadd(Hour, @TimeZone, lr.Created) as date) as Created
			  ,count(distinct lr.LeadId) as Quantity
		  FROM #leadresponses_temp lr inner join LeadContent lc on lc.LeadId = lr.LeadId-- where id > @minid
		  where lc.AffiliateSubId is not null and len(lc.AffiliateSubId) > 0
		  group by 
			  lr.[Status]
			  ,lc.[AffiliateSubId]
			  ,cast(dateadd(Hour, @TimeZone, lr.Created) as date)) s
			on (t.Created = s.Created and t.Status = s.Status and t.SubId = s.AffiliateSubId)
				WHEN NOT MATCHED BY TARGET
				THEN INSERT(Status, Created, Quantity, SubId) VALUES(s.Status, s.Created, s.Quantity, s.AffiliateSubId)
				WHEN MATCHED 
				THEN UPDATE SET t.Quantity = t.Quantity + s.Quantity;
					
	
	drop table #leadresponses_temp;

	update LeadMain set Status = 
	case
		when exists(select top 1 Id from LeadMainResponse where LeadId = LeadMain.Id and Status = 1) then 1
		else
		3
	end
	where Status = 4 and cast(Created as date) <= cast(dateadd(minute, -30, getutcdate()) as date);

	select 0;

END