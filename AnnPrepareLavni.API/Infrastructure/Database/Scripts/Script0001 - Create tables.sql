CREATE TABLE MedicalCondition (
    Id UUID PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    DateOfReport TIMESTAMP NOT NULL
);

CREATE TABLE Address (
    Id UUID PRIMARY KEY,
    Street VARCHAR(255) NOT NULL,
    City VARCHAR(100) NOT NULL,
    Country VARCHAR(100) NOT NULL
);

CREATE TYPE Gender AS ENUM ('Male', 'Female', 'Other'); -- Adjust according to your needs
CREATE TYPE HighestEducation AS ENUM ('None', 'Primary', 'Secondary', 'Tertiary', 'Quaternary'); -- Adjust according to your needs

CREATE TABLE Patient (
    Id UUID PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth TIMESTAMPTZ NOT NULL,
    Gender Gender NOT NULL,
    HeightInMeters REAL NOT NULL,
    WeightInKilograms REAL NOT NULL,
    FamilySize INT NOT NULL,
    HighestEducation HighestEducation,
    AddressId UUID NOT NULL REFERENCES Address(Id) ON DELETE SET NULL,
    CreatedAt TIMESTAMPTZ NOT NULL
);

CREATE TABLE PatientMedicalCondition (
    PatientId UUID REFERENCES Patient(Id) ON DELETE CASCADE,
    MedicalConditionId UUID REFERENCES MedicalCondition(Id) ON DELETE CASCADE,
    PRIMARY KEY (PatientId, MedicalConditionId)
);