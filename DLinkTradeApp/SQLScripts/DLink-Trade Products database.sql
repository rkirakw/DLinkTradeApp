create database if not exists productsdb;
use productsdb;

#Пункт выдачи
create table Storages(
	ID int auto_increment,
    Address varchar(100),
    constraint storage_pk primary key(ID)
);

create table ProductTypes(
	ID int auto_increment,
    TypeName varchar(50),
    constraint type_pk primary key(ID)
);

create table Products(
	ID int auto_increment,
    ProductName varchar(50),
    ProductType int,
    Cost int,
    ProductDescription text,
    Image mediumblob,
    constraint products_pk primary key(ID),
    constraint type_fk
    foreign key(ProductType)
    references ProductTypes(ID)
);

create table Users(
	ID int auto_increment,
    Login varchar(50),
    Pass varchar(100),
    Email varchar(100),
    Phone varchar(20),
    constraint users_pk primary key(ID)
);

create table Orders(
	ID int auto_increment,
    UserID int,
    ProductID int,
    StorageID int,
    Amount int,
    constraint orders_pk primary key(ID),
    constraint users_fk foreign key(UserID)
    references Users(ID),
    constraint products_fk foreign key(ProductID)
    references Products(ID),
    constraint storages_fk foreign key(StorageID)
    references Storages(ID)
);

