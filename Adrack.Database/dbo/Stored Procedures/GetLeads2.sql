-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeads2]
	-- Add the parameters for the stored procedure here
	@dateFrom datetime,
	@dateTo datetime,
	@Email nvarchar(50) = '',
	@AffiliateId varchar(MAX) = '',
	@AffiliateChannelId varchar(MAX) = '',
	@AffiliateChannelSubId nvarchar(50) = '',
	@BuyerId varchar(MAX) = '',
	@BuyerChannelId varchar(MAX) = '',
	@CampaignId varchar(MAX) = '',
	@Status smallint = -1,
	@IP nvarchar(20) = '',
	@FirstName nvarchar(50) = '',
	@LastName nvarchar(50) = '',
	@BPrice smallmoney = 0,
	@ZipCode nvarchar(50) = '',
	@State nvarchar(20) = '',
	@start int,
	@count int = 1,
	@leadId bigint = 0,
	@Notes nvarchar(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;

	IF OBJECT_ID('[TempDB].[dbo].[#AffiliateChannelsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#AffiliateChannelsId]
		END

	CREATE TABLE [dbo].[#AffiliateChannelsId]
	(	
		[Id] BIGINT NULL,
	)

	IF OBJECT_ID('[TempDB].[dbo].[#AffiliateIds]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#AffiliateIds]
		END

	CREATE TABLE [dbo].[#AffiliateIds]
	(	
		[Id] BIGINT NULL,
	)

	IF OBJECT_ID('[TempDB].[dbo].[#BuyerIds]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#BuyerIds]
		END

	CREATE TABLE [dbo].[#BuyerIds]
	(	
		[Id] BIGINT NULL,
	)


	IF OBJECT_ID('[TempDB].[dbo].[#BuyerChannelIds]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#BuyerChannelIds]
		END

	CREATE TABLE [dbo].[#BuyerChannelIds]
	(	
		[Id] BIGINT NULL,
	)

	IF OBJECT_ID('[TempDB].[dbo].[#CampaignIds]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#CampaignIds]
		END

	CREATE TABLE [dbo].[#CampaignIds]
	(	
		[Id] BIGINT NULL,
	)

	INSERT INTO [dbo].[#AffiliateChannelsId] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@AffiliateChannelId, ',');

	INSERT INTO [dbo].[#AffiliateIds] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@AffiliateId, ',');

	INSERT INTO [dbo].[#BuyerIds] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@BuyerId, ',');

	INSERT INTO [dbo].[#BuyerChannelIds] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@BuyerChannelId, ',');

	INSERT INTO [dbo].[#CampaignIds] 
	SELECT [Item] 
	FROM [dbo].SplitInts(@CampaignId, ',');


		SELECT /*distinct*/ LeadMain.*, LeadContent.*, LeadMainResponse.BuyerId, LeadMainResponse.BuyerChannelId, RedirectUrl.Clicked, RedirectUrl.Ip, RedirectUrl.Url, LeadMainResponse.AffiliatePrice, LeadMainResponse.BuyerPrice  FROM LeadMain
		INNER JOIN LeadContent ON LeadMain.Id = LeadContent.LeadId 
		LEFT JOIN LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId AND ((@BuyerChannelId = '' and @BuyerId = '' and LeadMainResponse.Status=1) or ((len(@BuyerChannelId) > 0 or len(@BuyerId) > 0) and LeadMainResponse.Status<=3))
		LEFT JOIN RedirectUrl ON RedirectUrl.LeadId = LeadMain.Id AND RedirectUrl.Clicked=1
		WHERE  LeadMain.Created BETWEEN @dateFrom AND @dateTo

		AND (@leadId = 0 or LeadMain.Id = @leadId)
		AND (@Status = -1 OR LeadMain.Status = @Status)
		AND (@State = '' OR LeadContent.State like '%'+@State+'%')
		AND (@IP = '' OR LeadContent.Ip like '%'+@IP+'%')
		AND (@Email = '' OR LeadContent.Email like '%'+@Email+'%')

		AND (@FirstName = '' OR LeadContent.Firstname like '%'+@FirstName+'%')
		AND (@LastName = '' OR LeadContent.Lastname like '%'+@LastName+'%')
		AND (@BPrice = 0 OR LeadMainResponse.BuyerPrice = @BPrice)
		AND (@ZipCode = '' OR LeadContent.Zip like '%'+@ZipCode+'%')

		AND (@AffiliateId = '' OR LeadMain.AffiliateId in (select id from [#AffiliateIds]))
		AND (@AffiliateChannelId = '' OR LeadMain.AffiliateChannelId in (select id from [#AffiliateChannelsId]))
		AND (@AffiliateChannelSubId = '' OR LeadContent.AffiliateSubId like '%'+@AffiliateChannelSubId+'%')
		AND (@BuyerId = '' OR LeadMainResponse.BuyerId in (select id from [#BuyerIds]))
		AND (@BuyerChannelId = '' OR LeadMainResponse.BuyerChannelId in (select id from [#BuyerChannelIds]))
		AND (@CampaignId = '' OR LeadMain.CampaignId in (select id from [#CampaignIds]))
		AND (len(@Notes) = 0 or (len(@Notes) > 0 and LeadMain.Id in (select LeadId from LeadNote where Note like '%' + @Notes + '%' or NoteTitleId in (select Id from NoteTitle where Title like '%' + @Notes + '%'))))

		ORDER BY LeadMain.Created Desc
		OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;
END