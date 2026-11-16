-- Escape Room Management System: database setup.
-- Run this whole script once in SQL Server Management Studio.
IF DB_ID('EscapeRoomDB') IS NULL
    CREATE DATABASE EscapeRoomDB;
GO
USE EscapeRoomDB;
GO
CREATE TABLE Staff
(
    StaffID INT
    IDENTITY(1,1)
    PRIMARY KEY,

    FullName NVARCHAR(100)
    NOT NULL,

    Username NVARCHAR(50)
    NOT NULL UNIQUE,

    Password NVARCHAR(50)
    NOT NULL,

    Role NVARCHAR(50)
    NOT NULL,

    IsActive BIT
    DEFAULT 1
);
GO
CREATE TABLE Rooms
(
    RoomID INT
    IDENTITY(1,1)
    PRIMARY KEY,

    RoomName NVARCHAR(100)
    NOT NULL,

    Theme NVARCHAR(100)
    NOT NULL,

    Difficulty NVARCHAR(50)
    NOT NULL,

    Capacity INT
    NOT NULL,

    PricePerSession DECIMAL(10,2)
    NOT NULL,

    IsAvailable BIT
    DEFAULT 1
);
GO

CREATE TABLE Bookings
(
    BookingID INT
    IDENTITY(1,1)
    PRIMARY KEY,

    RoomID INT
    NOT NULL,

    CustomerName NVARCHAR(100)
    NOT NULL,

    CustomerPhone NVARCHAR(20)
    NOT NULL,

    BookingDate DATETIME
    NOT NULL,

    GroupSize INT
    NOT NULL,

    PaymentStatus NVARCHAR(50)
    NOT NULL,

    TotalPrice DECIMAL(10,2)
    NOT NULL,

    CONSTRAINT FK_Bookings_RoomID
    FOREIGN KEY (RoomID)
    REFERENCES Rooms(RoomID)
);
GO

INSERT INTO Staff
VALUES
(
    'Administrator',
    'admin',
    'admin123',
    'Manager',
    1
);

INSERT INTO Staff
VALUES
(
    'Hussein Hany',
    'hussein',
    'ChangeMe1',
    'Manager',
    1
);

INSERT INTO Staff
VALUES
(
    'Ahmed Walid',
    'ahmed',
    'ChangeMe1',
    'Manager',
    1
);

INSERT INTO Staff
VALUES
(
    'Eslam Mohamed',
    'elsam',
    'ChangeMe1',
    'Manager',
    1
);
GO

INSERT INTO Staff
VALUES
(
    'Begad Sherif',
    'Begad',
    'ChangeMe1',
    'Manager',
    1
);
GO

INSERT INTO Rooms
VALUES
('The Haunted Mansion','Horror','Hard',6,2500,1),
('The Jungle Temple','Adventure','Medium',8,1800,1),
('The Cyber Vault','Sci-Fi','Hard',5,3000,1),
('Zombie Escape','Horror','Easy',4,1200,1),
('Secret Prison','Mystery','Medium',7,2200,1),
('Lost Pyramid','Adventure','Hard',5,2800,1),
('Alien Invasion','Sci-Fi','Medium',6,2100,1),
('Magic Castle','Fantasy','Easy',4,1500,1),
('Sherlock Mystery','Mystery','Hard',5,2700,1),
('Bank Heist','Crime','Medium',8,2400,1);
GO

INSERT INTO Bookings
VALUES
(1,'Ahmed Hassan','01012345678','2026-06-01',6,'Paid',2500),
(2,'Mohamed Ali','01123456789','2026-06-03',8,'Paid',1800),
(3,'Sarah Ahmed','01234567891','2026-06-05',3,'Pending',600),
(4,'Omar Khaled','01098765432','2026-06-06',6,'Paid',2400),
(5,'Mariam Adel','01111111111','2026-06-08',4,'Cancelled',1200),
(6,'Youssef Tarek','01222222222','2026-06-09',7,'Paid',3150),
(7,'Nour Hassan','01033333333','2026-06-10',5,'Paid',1400),
(8,'Ali Mahmoud','01144444444','2026-06-11',6,'Pending',2520),
(9,'Fatma Samy','01255555555','2026-06-12',4,'Paid',720),
(10,'Hussein Ahmed','01066666666','2026-06-13',5,'Paid',1350);
GO

SELECT * FROM Staff;
SELECT * FROM Rooms;
SELECT * FROM Bookings;
GO

INSERT INTO Rooms
VALUES
('Pirate Ship Escape','Adventure','Medium',6,2200,1),
('Pharaoh''s Tomb','Adventure','Hard',5,2900,1),
('Haunted Hospital','Horror','Hard',4,3100,1),
('Secret Laboratory','Sci-Fi','Medium',6,2600,1),
('The Cursed Circus','Horror','Medium',7,2300,1),
('Ancient Castle','Mystery','Hard',5,2800,1),
('The Lost Island','Adventure','Easy',8,1800,1),
('Space Mission','Sci-Fi','Hard',4,3200,1),
('Detective Office','Mystery','Easy',6,1700,1),
('Dragon Cave','Fantasy','Medium',7,2500,1);
GO