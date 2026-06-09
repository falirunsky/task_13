CREATE DATABASE PhoneBookDB;
GO

USE PhoneBookDB;
GO

CREATE TABLE Contacts
(
    Id    INT           PRIMARY KEY IDENTITY(1,1),
    Name  NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20)  NOT NULL
);
GO

INSERT INTO Contacts (Name, Phone) VALUES
    (N'Иван Иванов',    '+79001234567'),
    (N'Мария Петрова',  '+79009876543');
GO
