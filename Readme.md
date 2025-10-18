# SQLiteTutorials2 

A simple TaskItem app built with **.NET MAUI** and **SQLite**. This project demonstrates how to create a cross-platform mobile and desktop application using .NET MAUI with local data persistence via SQLite.

## Features

- Add new tasks
- Edit existing tasks
- Mark tasks as completed
- Delete tasks
- Persist data locally using SQLite
- Responsive UI for Android, iOS, Windows, and macOS

## Technologies Used

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/)
- [SQLite-net](https://github.com/praeclarum/sqlite-net) (lightweight ORM for SQLite)
- C#
- MVVM design pattern

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/SQLiteTutorials2.git



   Notes:
•	New columns are added automatically by sqlite-net when CreateTableAsync<TaskItem>() runs; indexes are created to keep queries fast.
•	Search matches both Title and Tags. Tag filter accepts comma-separated tokens and matches any of them.
•	Due filters use date ranges so it works with DateTime values stored by sqlite-net.
•	Sorting supports CreatedAt, DueDate (NULLs last), Priority, and Title with ascending/descending toggle.

## made by Tashwill, 2025  , Bellville , University of the Western Cape
