CREATE PROCEDURE dbo.addRows 
	@rowsNumber int,
	@blacklisttypeid int
AS
BEGIN
	SET NOCOUNT ON
	-- start point for adding rows
	--DECLARE @id INT = ISNULL((SELECT MAX(id) FROM dbo.Table1)+1, 1)
	DECLARE @iteration INT = 0
	WHILE @iteration < @rowsNumber
		BEGIN
			--get a random int from 0 to 100
			DECLARE @number INT = CAST(RAND()*100 AS INT)
			-- generate random nvarchar
			-- get a random nvarchar ascii char 65 to 128
			DECLARE @name NVARCHAR(150) = N'' --empty string for start
			DECLARE @length INT = 7--CAST(RAND() * 10 AS INT) --random length of nvarchar
			WHILE @length <> 0 --in loop we will randomize chars till the last one
				BEGIN
					set @name = @name + CHAR(CAST(RAND() * 63 + 65 as INT))
					SET @length = @length - 1 --next iteration
				END

			if @blacklisttypeid = 4
			begin
				set @name = @name + '@adrack.com';
			end;
			--insert data
			INSERT INTO dbo.BlackListValue(BlackListTypeId, [Value])
			VALUES (@blacklisttypeid, @name)
			SET @iteration += 1
			--SET @id += 1
		END
	SET NOCOUNT OFF
END