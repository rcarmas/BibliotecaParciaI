CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    UserType VARCHAR(20),
    Status VARCHAR(10)
);

CREATE TABLE Books (
    Id TEXT PRIMARY KEY,
    Title VARCHAR(200),
    Author VARCHAR(100),
    Category VARCHAR(50),
    Availability BOOLEAN,
    Location VARCHAR(100)
);

CREATE TABLE Transactions (
    Id SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(Id),
    BookId TEXT REFERENCES Books(Id),
    BorrowDate DATE,
    ReturnDate DATE,
    Status VARCHAR(20),
    Fine DECIMAL(10, 2)
);