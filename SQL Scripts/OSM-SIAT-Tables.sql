use [OSM-SIAT-P]  

create table Way
(
	id bigint not null,
	[type] nvarchar(20),
	ref nvarchar(20),
	name nvarchar(80),
	country nchar(2),
	oneway bit not null,
	geo geography
)

create table Node
(
	id bigint not null,
	wayId bigint not null,
	orderId smallint not null,
	lat float not null,
	lon float not null,
	geo geography,
)

alter table Way 
add primary key (id)

alter table Node 
add primary key (id,wayId)

alter table Node 
add constraint fk_WayNode FOREIGN KEY (wayId) REFERENCES Way (id) ON DELETE CASCADE 

ALTER TABLE Node NOCHECK CONSTRAINT ALL  
ALTER TABLE Way NOCHECK CONSTRAINT ALL  






