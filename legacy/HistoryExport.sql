use master
go

create database PointsHistory_MADPWEBI518
on     (name = PointsHistory_data, filename = 'D:\SQLSERVER\MSSQL14.S17EBI2\MSSQL\DATA\PointsHistory_MADPWEBI518.mdf')
log on (name = PointsHistory_log,  filename = 'D:\SQLSERVER\MSSQL14.S17EBI2\MSSQL\DATA\PointsHistory_MADPWEBI518.ldf')
GO

use PointsHistory_MADPWEBI518
go

set nocount on
go

create table Point
(
	PointId int,
	PointName NVARCHAR(50),
	ParamName nvarchar(50),
	Description nvarchar(500) null,
	Device nvarchar(100) null,
	HistoryFast BIT NULL,
	HistorySlow BIT NULL,
	HistoryExtd BIT NULL,
	HistoryFastArch BIT NULL,
	HistorySlowArch BIT NULL,
	HistoryExtdArch BIT NULL,
	constraint PK_Points primary key (PointId)
)
create unique index IX_Name ON Point (PointName, ParamName)
go

create Table History_5sec
(
	PointId int,
	USTTimestamp datetime,
	[Timestamp] datetime,
	Value float,
	constraint PK_History_5sec primary key (PointId, USTTimestamp),
	constraint FK_History_5sec_Point foreign key (PointId) references Point(PointId)
)
go

create Table History_1min
(
	PointId int,
	USTTimestamp datetime,
	[Timestamp] datetime,
	Value float,
	constraint PK_History_1min primary key (PointId, USTTimestamp),
	constraint FK_History_1min_Point foreign key (PointId) references Point(PointId)
)
go

create Table History_1hour
(
	PointId int,
	USTTimestamp datetime,
	[Timestamp] datetime,
	Value float,
	constraint PK_History_1hour primary key (PointId, USTTimestamp),
	constraint FK_History_1hour_Point foreign key (PointId) references Point(PointId)
)
go

create Table Parameter
(
	Name nvarchar(30),
	Value nvarchar(20),
	Comment nvarchar(100) null,
	constraint PK_Parameter primary key (Name)
)
go


/****** Object:  Index [IX_History_1hour]    Script Date: 2022-09-27 16.24.00 ******/
CREATE NONCLUSTERED INDEX [IX_History_1hour] ON [dbo].[History_1hour]
(
	[USTTimestamp] ASC
)
GO

/****** Object:  Index [IX_History_1min]    Script Date: 2022-09-27 16.24.00 ******/
CREATE NONCLUSTERED INDEX [IX_History_1min] ON [dbo].[History_1min]
(
	[USTTimestamp] ASC
)
GO

/****** Object:  Index [IX_History_5sec]    Script Date: 2022-09-27 16.24.00 ******/
CREATE NONCLUSTERED INDEX [IX_History_5sec] ON [dbo].[History_5sec]
(
	[USTTimestamp] ASC
)

