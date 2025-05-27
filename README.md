# Quan_ly_xe_hoi
![image](https://github.com/user-attachments/assets/ff95722c-4ba8-4924-9b6e-753a1c195cdf)
SQL:

create table Branch
(
    id      int identity
        constraint PK_Branch
            primary key,
    name    nvarchar(100) not null,
    country nvarchar(50)  not null
)
go

create table ElectricCar
(
    id        int identity
        constraint PK_ElectricCar
            primary key,
    battery   float          not null,
    range_km  int            not null,
    model     nvarchar(100)  not null,
    branch_id int            not null
        constraint FK_ElectricCar_Branch_branch_id
            references Branch
            on delete cascade,
    price     decimal(10, 2) not null,
    ImageUrl  nvarchar(500)
)
go

create index IX_ElectricCar_branch_id
    on ElectricCar (branch_id)
go

create table GasCar
(
    id        int identity
        constraint PK_GasCar
            primary key,
    fuel_eff  float          not null,
    model     nvarchar(100)  not null,
    branch_id int            not null
        constraint FK_GasCar_Branch_branch_id
            references Branch
            on delete cascade,
    price     decimal(10, 2) not null,
    ImageUrl  nvarchar(500)
)
go

create index IX_GasCar_branch_id
    on GasCar (branch_id)
go

create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null
        constraint PK___EFMigrationsHistory
            primary key,
    ProductVersion nvarchar(32)  not null
)
Go

