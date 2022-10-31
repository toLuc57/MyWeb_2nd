create database MyWeb
go

use MyWeb
go

create table [dbo].[User] (
	[Id] int IDENTITY (1,1) not null primary key,
	[Email] varchar(50),
	[Phone] varchar(12),
	[Username] varchar(50),
    [Password] varchar(50),
    [Roles] varchar(50),
    [CreateBy] int,
    [CreateAt] datetime,
    [UpdateBy] int,
    [UpdateAt] datetime,
    [DeleteBy] int,
    [DeleteAt] datetime
)

create table [dbo].[Category](
	[Id] int IDENTITY (1,1) not null primary key,
	[Name] nvarchar(50),
	[Slug] nvarchar(50),
	[ParentId] int,
	[Orders] int,
	[Metakey] nvarchar(50),
	[Metadesc] nvarchar(50),
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[Contact] (
	[Id] int IDENTITY (1,1) not null primary key,
	[FullName] nvarchar(50),
	[Email] nvarchar(50),
	[Phone] nvarchar(12),
	[Title] nvarchar(50),
	[Content] nvarchar(50),
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[Menu] (
	[Id] int IDENTITY (1,1) not null primary key,
	[Name] nvarchar(50),
	[Link] nvarchar(50),
	[Type] nvarchar(50),
	[TableId] int,
	[ParentId] int,
	[Orders] int,
	[Status] int
)

create table [dbo].[Order] (
	[Id] int IDENTITY (1,1) not null primary key,
	[UserId] int,
	[Name] nvarchar(50),
	[Phone] nvarchar(12),
	[Email] nvarchar(50),
	[Note] nvarchar(50),
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[OrderDetail] (
	[Id] int IDENTITY (1,1) not null primary key,
	[OrderId] int,
	[ProductId] int,
	[Price] decimal(12,3),
	[Qty] int,	
	[Amount] decimal(6,2)
)

create table [dbo].[Post] (
	[Id] int IDENTITY (1,1) not null primary key,
	[TopicId] int,
	[Title] nvarchar(50),
	[Slug] nvarchar(50),
	[Detail] nvarchar(MAX),
	[Metakey] nvarchar(50),
	[Metadesc] nvarchar(50),
	[Img] nvarchar(MAX),
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[Product] (
	[Id] int IDENTITY (1,1) not null primary key,
	[CatId] int,
	[Name] nvarchar(50),
	[Slug] nvarchar(50),
	[Detail] nvarchar(50),
	[Metakey] nvarchar(50),
	[Metadesc] nvarchar(50),
	[Img] nvarchar(MAX),
	[Number] int,
	[Price] decimal(12,3),
	[Pricesale] decimal(12,3),	
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[Slider] (
	[Id] int IDENTITY (1,1) not null primary key,
	[Name] nvarchar(50),
	[Link] nvarchar(MAX),
	[Img] nvarchar(MAX),
	[Orders] int,
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)

create table [dbo].[Topic] (
	[Id] int IDENTITY (1,1) not null primary key,
	[Name] nvarchar(50),
	[Slug] nvarchar(50),
	[ParentId] int,
	[Orders] int,
	[Metakey] nvarchar(50),
	[Metadesc] nvarchar(50),
	[CreateBy] int,
	[CreateAt] datetime,
	[UpdateBy] int,
	[UpdateAt] datetime,	
	[Status] int
)
