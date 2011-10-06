-- query procedures

-- GetClosestNode
create procedure GetClosestNode @lat float, @lon float
as
declare @h geography = geography::STPointFromText('Point(' + CONVERT(varchar,@lat) + ' ' + CONVERT(varchar, @lon)+ ')', 4326)
select Top(1) *  
from Node
where geo.STDistance(@h) < 500 
order by geo.STDistance(@h) asc
go

-- GetClosestWayNode
create procedure GetClosestWayNode @lat float, @lon float, @idWay bigint
as
declare @h geography = geography::STPointFromText('Point(' + CONVERT(varchar,@lat) + ' ' + CONVERT(varchar, @lon)+ ')', 4326)
select Top(1) *  
from Node
where wayId = @idWay
order by geo.STDistance(@h) asc
go

-- GetClosestWay
create procedure GetClosestWay @lat float, @lon float
as
declare @h geography = geography::STPointFromText('Point(' + CONVERT(varchar,@lat) + ' ' + CONVERT(varchar, @lon)+ ')', 4326)
select Top(1) *  
from Way
where @h.STBuffer(100).STIntersects(geo) = 1
go

