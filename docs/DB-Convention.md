# Database Convention

> This documents defines the database conventions and standards followed in the ThemePark@UCR project. It outlines the guidelines for designing, naming, and managing the database to ensure consistency, clarity, and maintainability.

---

## 1. Schemas and logical organization

There will be a single database with four schemas:
- `Buildings`: Contains all tables related to buildings and their attributes.
- `LearningSpaces`: Contains all tables related to learning spaces inside buildings and their attributes.
- `InteractiveComponents`: Contains all tables related to interactive components inside learning spaces and their attributes.
- `Users`: Contains all tables related to user management and authentication.

*The `dbo` schema should be avoided as much as possible.*

## 2. Naming Conventions

### Tables

- Use PascalCase for table and column names (e.g., `User` (table), `RoomNumber` (column), `Name` (column)).
- Use a singular noun for table names (e.g., `Building`, `LearningSpace`, `InteractiveComponent`).
- Use meaningful names and avoid abbreviations unless they are universally understood.

### Constraints

- Primary Key: `PK_<TableName>` (e.g., `PK_User`).
- Foreign Key: `FK_<TableName>_<ReferencedTableName>` (e.g., `FK_LearningSpace_Building`).
- Use `NOT NULL` always unless there is a specific reason to allow nulls.

*It is preferable to add foreign key constraints at the end of the `.sql` file for better organization, using `ALTER`.*

## 3. Data Types

- Text: `NVARCHAR(n)`. Choose `n` based on the expected maximum length of the text and avoid using `NVARCHAR(MAX)` unless absolutely necessary.
- Boolean: `BIT`.
- Date and Time: `DATETIME2` for date and time values.
- Numeric: Use appropriate numeric types like `INT`, `FLOAT`, or `DECIMAL` based on the precision and scale required.
- Identifiers: Use what is considered necessary based on the team's needs. 

## 4. Table Design (Example)

Suppose we need to create a table in one schema:

```sql
CREATE TABLE Schema1.EntityName (
	Id INT,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT PK_EntityName PRIMARY KEY (Id)
);
```

Now we need to create a table in another schema that references the previous table:

```sql
CREATE TABLE Schema2.AnotherEntity (
	Id INT,
	EntityNameId INT, -- Foreign key referencing Schema1.EntityName
	Title NVARCHAR(100) NOT NULL,
	CONSTRAINT PK_AnotherEntity PRIMARY KEY (Id)
);

ALTER TABLE Schema2.AnotherEntity ADD CONSTRAINT FK_AnotherEntity_EntityName FOREIGN KEY (EntityNameId) REFERENCES Schema1.EntityName(Id)
```

## 5. Conceptual Design

The conceptual design of the database is represented in the following Entity-Relationship Diagram (ERD):

[ERD](https://drive.google.com/file/d/1YgSt9RzztVnUupy46rieksh8GM3S2rY7/view?usp=sharing)

This ERD ilustrates the main entities related to the four main functionalities required by the project: Buildings, Learning Spaces, Interactive Components and Users. Each entity includes its attributes and relationships with other entities.

*Note: The ERD is subject to change as the project evolves. Always refer to the latest version for accurate information.*

## 6. Logical Design

The logical design of the database is represented in the following Relational Diagram:

[Relational Diagram](https://drive.google.com/file/d/1YgSt9RzztVnUupy46rieksh8GM3S2rY7/view?usp=sharing)

This diagram illustrates the tables, their columns, primary keys and foreign key relationships (including composite keys).

*Note: The logical design is subject to change as the project evolves. Always refer to the latest version for accurate information.*

---
[↩️ Back to Technical Decisions README](./05-technical-decisions.md)