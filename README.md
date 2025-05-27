
# ToDoApp

A modern, cross-platform mobile task management app built with .NET MAUI.  
**Features a beautiful login/register experience, Google sign-in, and a clean dashboard for managing your tasks.**

---

## Features

- **User Authentication**
  - Email/password login and registration
  - Google login (UI-ready)
  - Secure storage of user data

- **Task Management**
  - Add, edit, complete, and delete tasks
  - Separate views for active and completed tasks
  - Pull-to-refresh and real-time updates

- **Profile**
  - View user info and task statistics
  - Logout and (future) profile editing

- **Modern UI/UX**
  - Pixel-perfect login/register screens
  - Loading indicators for smooth transitions
  - Responsive, grid-based layouts
  - Consistent branding and color scheme

- **Platform**
  - Android

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022+ with MAUI workload
- Android/iOS emulator or device

### Build & Run

1. **Clone the repository**
   ```sh
   git clone <your-repo-url>
   cd ToDoApp
   ```

2. **Restore dependencies**
   ```sh
   dotnet restore
   ```

3. **Build and run**
   - For Android:
     ```sh
     dotnet build -t:Run -f net8.0-android
     ```
   - For iOS/MacCatalyst/Windows, use the appropriate target framework.

4. **Login/Register**
   - The app always opens at the login page.
   - Register a new account or use your credentials to log in.

---

## Project Structure

- `LoginPage.xaml` / `RegisterPage.xaml` — Authentication UI
- `DashboardPage.xaml` — Main task dashboard
- `CompletedDashboardPage.xaml` — Completed tasks
- `ProfilePage.xaml` — User profile and stats
- `Services/` — API and authentication logic
- `Models/` — Data models




---


**Built with ❤️ using .NET MAUI.**

---

Let me know if you want a version with setup screenshots, API details, or anything else!
