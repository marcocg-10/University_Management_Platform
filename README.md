<div align="center">

# Universidad de Costa Rica
## School of Computer Science and Informatics
### Software Engineering & Database Integration Project

# Themepark@UCR
## Conceptualization Document
### Version 1.0 - Updated September 17, 2025

---

## Product Vision

For current and future university community members and general public who want to explore, learn and experience about universities in an interactive way, the ThemePark@UCR is a Web VR portal that universalizes the university experience through immersive means. Unlike monotone virtual classrooms and traditional methods, our product offers a complete, immersive and interactive experience.

---

## 📌 Group 01 – Software Engineering & Database Integration Project

</div>

## 📑 Table of Contents

1. [Definitions, Acronyms, and Abbreviations](#1-definitions-acronyms-and-abbreviations)
2. [Introduction](#2-introduction)
   - Document Purpose and Objectives
   - Document Structure
3. [Teams and Members](#3-teams-and-members)
   - [Role Assignments by Iteration](#3-role-assignments-by-iteration)
4. [System Overview](#4-system-overview)
   - [Context and Current Situation](#4-context-and-current-situation)
   - [Problem Statement](#4-problem-statement)
   - [Stakeholders and User Types](#4-stakeholders-and-user-types)
   - [Proposed Solution](#proposed-solution)
   - [Environment Analysis](#environment-analysis)
     - Business Strategy
     - System Objectives
     - Expected Usage
     - Legacy Systems
     - Regulatory Aspects
     - Business Assumptions and Constraints
     - Existing Solutions
   - [Product Vision](#product-vision)
   - [External Systems Integration](#external-systems-integration)
   - [Modules and Epics](#modules-and-epics)
   - [Functional Requirements](#functional-requirements)
   - [Product Roadmap](#product-roadmap)
   - [Non-functional Requirements](#non-functional-requirements)
5. [Technical Decisions](#technical-decisions)
   - [Methodologies and Defined Processes](#methodologies-and-defined-processes)
   - [Project Development Artifacts](#project-development-artifacts)
   - [Technologies and Versions](#technologies-and-versions)
   - [Code Repository and Git Strategy](#code-repository-and-git-strategy)
   - [Definition of Done (DoD)](#definition-of-done)
   - [Data Requirements](#data-requirements)
   - [Database Conceptual Design](#database-conceptual-design)
   - [Database Logical Design](#database-logical-design)
6. [References](#references)

---

# 1. Definitions, Acronyms, and Abbreviations
In the following document, the definitions, acronyms and abbreviations are listed and explained.

📄 [View complete section](./docs/01-definitions.md)

# 2. Introduction
## Document Purpose and Objectives
This document captures the purpose, scope and organization of the ThemePark@UCR project documentation. It provides stakeholders, developers and contributors with a clear description of the goals, expected outcomes, and the structure of the materials that support design, development, testing and delivery of the Web VR portal.

Primary objectives:

- Communicate the product vision and high-level goals for ThemePark@UCR.
- Define project stakeholders, user types and responsibilities.
- Provide a single source of truth for system overview, requirements (functional and non-functional), technical decisions and data design.
- Serve as the reference for iteration planning, role assignments, and deliverable acceptance criteria.

## Document Structure

This documentation is organized to support both quick orientation and deep dives. Top-level sections include:

- Definitions, Acronyms, and Abbreviations — terminology used across the project.
- Introduction — purpose, objectives and how to navigate the documentation.
- Teams and Members — team composition and role assignments by iteration.
- System Overview — context, problem statement, stakeholders, solution and environment analysis.
- Technical Decisions — methodologies, artifacts, technologies, git strategy, DoD and database design.
- References — bibliographic and external links.

Each top-level section has its dedicated markdown file in the `docs/` folder for detailed content and updates.

# 3. Teams and Members

In this section, there's a detailed list of each team member and their role for each sprint, followed by the transversal teams that develop this project.


📄 [View complete section](./docs/03-teams.md)

# 4. System Overview

In the following section there's a system overview contemplating the varying elements of the project.


📄 [View complete section](./docs/04-system-overview.md)

# 5. Technical Decisions

In the technical decisions section theres an overview of methodologies used, technologies used, repositories and git strategy, among other details relevant to the project.

📄 [View complete section](./docs/05-technical-decisions.md)

# 6. References

In this section, all bibliographic references and external links used throughout the project are listed.

📄 [View complete section](./docs/06-references.md)

---

# Development Information

## 🏗 Project Structure

The solution is organized into several projects following clean architecture principles:

### Backend Projects

- **Backend.Domain** - Domain layer
  - Contains enterprise business rules and entities
  - Defines core business logic and domain models
  - Implements domain-driven design patterns

- **Backend.Application** - Application layer
  - Implements application-specific business rules
  - Contains use cases and application services
  - Orchestrates domain objects to perform tasks

- **Backend.Api** - Main Web API entry point
  - Handles HTTP requests and responses
  - Contains API controllers and endpoints
  - Manages application configuration and startup

- **Backend.Infrastructure** - Infrastructure layer
  - Implements data access and external service integrations
  - Contains repository implementations
  - Handles database contexts and migrations

- **Backend.Presentation.Api** - Presentation layer
  - Contains API-specific logic and models
  - Manages API versioning and documentation
  - Handles request/response models (DTOs)

### Test Projects

- **Backend.Infrastructure.Tests.Unit** - Unit tests for Infrastructure layer
- **Backend.Presentation.Api.Tests.Unit** - Unit tests for Presentation layer

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code with C# extensions

### Building the Solution

```bash
dotnet build
```

### Running the Tests

```bash
dotnet test
```

### Running the API

```bash
cd Backend.Api
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5264
- HTTPS: https://localhost:7119

---



✍ Last updated: September 17th, 2025

