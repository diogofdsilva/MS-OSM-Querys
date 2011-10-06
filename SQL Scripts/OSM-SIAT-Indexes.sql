
-- create clustered index on Node(wayId)

GO
CREATE NONCLUSTERED INDEX [NodeWayIdIndex] ON [dbo].[Node] 
(
	[wayId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- create index on Node(geo)

GO
CREATE SPATIAL INDEX [NodeGeoIndex] ON [dbo].[Node] 
(
	[geo]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = HIGH,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

-- create index on Way(geo)


GO
CREATE SPATIAL INDEX [WayGeoIndex] ON [dbo].[Way] 
(
	[geo]
)USING  GEOGRAPHY_GRID 
WITH (
GRIDS =(LEVEL_1 = MEDIUM,LEVEL_2 = MEDIUM,LEVEL_3 = MEDIUM,LEVEL_4 = MEDIUM), 
CELLS_PER_OBJECT = 16, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

--rebuild all indexes

GO
ALTER INDEX [NodeWayIdIndex] ON [dbo].[Node] REBUILD PARTITION = ALL WITH ( PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, ONLINE = OFF, SORT_IN_TEMPDB = OFF )
GO
ALTER INDEX [NodeGeoIndex] ON [dbo].[Node] REBUILD PARTITION = ALL WITH ( PAD_INDEX  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, ONLINE = OFF, SORT_IN_TEMPDB = OFF )
GO
ALTER INDEX [WayGeoIndex] ON [dbo].[Way] REBUILD PARTITION = ALL WITH ( PAD_INDEX  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, ONLINE = OFF, SORT_IN_TEMPDB = OFF )
GO

