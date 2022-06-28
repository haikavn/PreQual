-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE GenerateLeads
(
    -- Add the parameters for the stored procedure here
	@count int,
	@startFromDays int = 0,
	@addMilliseconds int = 0
)
AS
BEGIN
	declare @index int = 1;
	declare @startDate DateTime = getutcdate();

	set @startDate = dateadd(day, @startFromDays, @startDate);

	while(@index <= @count)
	begin
		exec dbo.GenerateLead @startDate;

		declare @coef decimal = 1;
		declare @UpperCoef int = 30;
		declare @LowerCoef int = 1;

		set @coef = ROUND(((@UpperCoef - @LowerCoef - 1) * RAND() + @LowerCoef), 0);

		print @coef;

		set @startDate = DATEADD(millisecond, @addMilliseconds * (@coef / 10.0), @startDate);

		if @startDate > getutcdate()
		begin
			return;
		end;

		set @index = @index + 1;
	end;

END