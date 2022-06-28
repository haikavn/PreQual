-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDublicateLeadByBuyer] 
	-- Add the parameters for the stored procedure here
	@ssn varchar(150),
	@Created DateTime,
	@Id bigint,
	@fromBuyer bit,
	@leadId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

if exists (SELECT top 1 [dbo].[LeadMainResponse].Id
  FROM [dbo].[LeadMainResponse] where 
  
  ((@leadId > 0 and dbo.LeadMainResponse.LeadId = @leadId) or 
  (@leadId = 0 and dbo.LeadMainResponse.Ssn = @ssn and dbo.LeadMainResponse.Created >= @Created)) and 

  ((@fromBuyer = 0 and dbo.LeadMainResponse.BuyerChannelId = @Id) or (@fromBuyer = 1 and dbo.LeadMainResponse.BuyerId = @Id)))
  begin
	select 1;
  end
  else
  begin
	select 0;
  end;
END