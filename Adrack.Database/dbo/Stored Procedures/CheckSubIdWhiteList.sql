-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[CheckSubIdWhiteList]
	-- Add the parameters for the stored procedure here
	@subid varchar(50),
	@buyerchannelid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if ( select ISNULL(count(Id), 0) from SubIdWhiteList where subid = @subid and buyerchannelid = @buyerchannelid) > 0
	begin
		select 1;
	end
	else
	begin
		select 0;
	end;
END