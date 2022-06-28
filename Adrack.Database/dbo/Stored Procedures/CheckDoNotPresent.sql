-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[CheckDoNotPresent]
	-- Add the parameters for the stored procedure here
	@email varchar(50),
	@ssn varchar(50),
	@buyerid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if ( select count(t.id) from (SELECT * FROM DoNotPresent WHERE buyerid = @buyerid and (expirationdate is null or (expirationdate is not null and expirationdate > getutcdate() ))) as t where @email  = t.email and @ssn = t.ssn  ) > 0
	begin
		select 1;
	end
	else
	begin
		select 0;
	end;

	--and @email like email + '%' and @ssn like '%' + ssn 

	--declare @leftEmail varchar(50) = LEFT(@email, 1);
	--declare @rightSsn varchar(50) = RIGHT(@ssn, 1);

    -- Insert statements for procedure here
	--if ( select count(t.id) from (SELECT * FROM DoNotPresent WHERE buyerid = @buyerid and @leftEmail = LEFT(email, 1) and @rightSsn = RIGHT(ssn, 1) and (expirationdate is null or (expirationdate is not null and expirationdate > getutcdate() ))) as t where @email like t.email + '%' and @ssn like '%' + t.ssn  ) > 0
	--begin
		--select 1;
	--end
	--else
	--begin
		--select 0;
	--end;
END