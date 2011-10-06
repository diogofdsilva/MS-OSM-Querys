
-- create geographic builder procedures

create procedure GenerateNodePoints 
as
DECLARE @Cursor CURSOR
DECLARE @currId bigint
DECLARE @currLat float
DECLARE @currLon float
declare @st nvarchar(25)
SET NOCOUNT ON 
SET @Cursor = CURSOR FOR SELECT id,lat,lon FROM Node

OPEN @Cursor

-- Perform the first fetch.
FETCH NEXT FROM @Cursor
into @currId,@currLat,@currLon
set @st = 'Point(' + cast((@currLat) as varchar(12)) + ' ' + cast((@currLon) as varchar(12)) + ')'

update Node set geo = @st where id = @currId	

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	-- This is executed as long as the previous fetch succeeds.
	FETCH NEXT FROM @Cursor
	into @currId,@currLat,@currLon
	
	set @st = 'Point(' + cast((@currLat) as varchar(12)) + ' ' + cast((@currLon) as varchar(12)) + ')'
	
	update Node set geo = @st where id = @currId	
END
close @Cursor
DEALLOCATE @Cursor
SET NOCOUNT OFF 
GO


create procedure GenerateWayLine 
as
DECLARE @CursorWays CURSOR
DECLARE @CursorNodes CURSOR
DECLARE @currWayId bigint
DECLARE @currLat float
DECLARE @currLon float
declare @st nvarchar(MAX)
SET NOCOUNT ON 
SET @CursorWays = CURSOR FOR SELECT id FROM Way

OPEN @CursorWays

-- Perform the first fetch.
FETCH NEXT FROM @CursorWays
into @currWayId

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @CursorNodes = CURSOR FOR SELECT lat,lon FROM Node where wayId = @currWayId order by orderId ASC
	
	OPEN @CursorNodes
	-- fetch first node
	
	FETCH NEXT FROM @CursorNodes
	into @currLat, @currLon
	
	SET @st = 'LINESTRING(' + cast((@currLat) as varchar(12)) + ' ' + cast((@currLon) as varchar(12))
	
	-- fetch rest node
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		FETCH NEXT FROM @CursorNodes
		into @currLat, @currLon
		
		SET @st = @st + ',' + cast((@currLat) as varchar(12)) + ' ' + cast((@currLon) as varchar(12))
		
	END
	CLOSE @CursorNodes
	DEALLOCATE @CursorNodes
	
	SET @st = @st + ')'
	
	update Way set geo = @st where id = @currWayId	
	
	-- This is executed as long as the previous fetch succeeds.
	FETCH NEXT FROM @CursorWays
	into @currWayId
	
END
close @CursorWays
DEALLOCATE @CursorWays
SET NOCOUNT OFF 
GO


-- execute geographic builder scripts
exec GenerateWayLine
exec GenerateNodePoints


-- reactivate table contraints
ALTER TABLE Node CHECK CONSTRAINT ALL  
ALTER TABLE Way CHECK CONSTRAINT ALL  