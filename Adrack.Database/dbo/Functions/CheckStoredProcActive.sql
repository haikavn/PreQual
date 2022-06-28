
CREATE FUNCTION [dbo].[CheckStoredProcActive] (@StoredProc nvarchar(255))

RETURNS int

as

begin

declare @spCount int;

set @spCount=(select count(*) FROM sys.dm_exec_sessions s

INNER JOIN sys.dm_exec_requests r

ON r.session_id=s.session_id

CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) AS SQL

INNER JOIN sys.objects o

ON SQL.objectid=o.object_id

WHERE  s.is_user_process= 1

AND r.database_id=db_id()

AND o.name=@StoredProc);

RETURN @spCount;

end;