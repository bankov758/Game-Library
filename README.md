# Game Library

## Overview

This ASP.NET MVC project is designed to be a comprehensive platform for game enthusiasts. 
It allows users to manage a personal library of video games, complete with the ability to add detailed 
information and images for each title. The platform includes user authentication features with login and registration functionality. 
Additionally, users can connect with friends, compare gaming statistics, and share their game libraries.

## Features

### User Authentication
- **Registration**: New users can register by providing a username, email, and password.
- **Login**: Existing users can log in using their credentials.

### Game Library Management
- **Add Games**: Users can add new games to their library, including title, description, genre, release date, and images.
- **Edit Games**: Users can edit information about games in their library.
- **Delete Games**: Users can remove games from their library.

### Social Features
- **Add Friends**: Users can send and accept friend requests.
- **Compare Stats**: Users can compare their game statistics, such as playtime and achievements, with their friends.

### User Profile
- **Profile Management**: Users can view and edit their profile, including changing their password and profile picture.

## Technologies Used
- **ASP.NET MVC**: For creating the web application.
- **Bootstrap**: For responsive UI design.
- **SQL Server**: As the backend database.

## Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7 or later
- MSSQL Server 2019 or later

### Setup
1. **Clone the repository**.
2. **Open the solution in Visual Studio**.
3. **Restore NuGet packages**.
4. **Set up the database**:
   - Update the connection string in `web.config`.
   - Run the create_db script in App_Data/Db.
5. **Run the application**: Start the application in Visual Studio to launch the web server.

## Database