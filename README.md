# Data Analyst Importer - Simple and Versatile Import Tool for Analytical Software

![Project Image](https://github.com/tomasKopecky/tomasKopecky/blob/main/media/repos_images/AnalystDataImporter/banner_speed_3.gif)

---

## Introduction 💡
Data Analyst Importer is a WPF application developed in .NET C# adhering to the MVVM design pattern. It's designed for importing structured data from csv or txt files, allowing analysts (especially from the law enforcement branch) to create and manipulate graphic charts representing objects and relations. Users can manage import templates and export data to analytical software like I2 Analyst's Notebook or Tovek Tools.

The application is being developed with features like Dependency Injection, Behaviors, and Factories to ensure modularity and maintainability.

## Technologies Used 📖

<img src="https://github.com/tomasKopecky/tomasKopecky/blob/main/media/icons/csharp/csharp-original.svg" width="55"><img src="https://github.com/tomasKopecky/tomasKopecky/blob/main/media/icons/dot-net/dot-net-original.svg" width="65"><img src="https://github.com/tomasKopecky/tomasKopecky/blob/main/media/icons/postgresql/postgresql-original.svg" width="50"><img src="https://github.com/tomasKopecky/tomasKopecky/blob/main/media/icons/sqlite/sqlite-original.svg" width="50">

The application is built using:
- .NET C# WPF: A popular programming language/platform/framework for Windows and web applications.
- PostgreSQL/SQLite: Used for server/local data storage and testing.
- MVVM Design Pattern: Ensures a clean separation of concerns.
- Additional patterns and tools like Dependency Injection, Behaviors, Factories, etc.

## Features 🌟
**Current Features Include:**
- Importing structured data from csv or txt files.
- Modeling data into objects and relations within a graphic chart, with capabilities to:
  - Edit object icons.
  - Edit relation styles (color, thickness, etc.).
  - Modify properties like labels, dates, and times.
- Exporting data to analytical software like I2 Analyst's Notebook or Tovek Tools.
- Managing graphic chart templates and export targets.

**Future Development Plans:**
- Transition from SQLite to PostgreSQL for enhanced scalability.
- Integrate Entity Framework for object-relational mapping.
- Expand the library of icons and graphical editing options.
- Enhance data manipulation functions (character replacement, adding prefixes, etc.).

## Screenshots and Demo 📸

Watch the GIF in the heading of this repo.

## Setup Instructions 🛠️

1. Clone this repository.
2. Install .NET dependencies and ensure the .NET SDK is set up on your system.
3. Build and run the application through Visual Studio or a suitable .NET IDE.

## Developer Notes 🔑

Some parts of the application have been anonymized due to restrictions by the Police of the Czech Republic as well as the current SQLite database for the 1.0.0_presentation branch. Access to the full source code or specific functionalities is restricted and available only under legal circumstances.

## Project Origins 🏛️

This project is under development with a focus on aiding law enforcement analysts in data handling and analysis, promoting efficient and user-friendly workflows in law enforcement analysis.