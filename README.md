<p align="right">
  <strong>English</strong> | <a href="./README.zh-CN.md">简体中文</a>
</p>

# TrackLab

TrackLab is a .NET 8 and WPF prototype for monitoring equipment modules. It visualizes module states, displays digital inputs and outputs (DI/DO), and demonstrates a modular approach to building desktop applications.

The project uses Caliburn.Micro for MVVM and navigation. Separate Core, UI, Client, and Simulator projects keep domain logic, reusable UI components, and executable applications isolated from one another.

## Features

- Display hot plates, cool plates, load ports, and a transfer robot in a configurable equipment layout
- Visualize module states with status colors and open a detail dialog by clicking a module
- Inspect DI/DO points and their current values by module
- Configure application menus with JSON and the equipment layout with XAML
- Reuse shared controls, vector icons, converters, and theme resources
- Extend a separate simulator application for future equipment simulation

> TrackLab is currently a prototype. Module and IO data are initialized in memory when the client starts; no real equipment, database, or persistent configuration source is connected yet.

## Tech Stack

| Technology | Purpose |
| --- | --- |
| .NET 8 | Application runtime and base libraries |
| WPF | Windows desktop UI |
| Caliburn.Micro 4.0.230 | MVVM, view location, and lifecycle management |
| Newtonsoft.Json 13.0.4 | Menu configuration deserialization |

## Repository Structure

```text
TrackLab/
├─ TrackLab.Client/       # Main application, pages, ViewModels, and startup data
│  ├─ Config/             # Menu and equipment layout configuration
│  └─ View/               # Application views and their ViewModels
├─ TrackLab.Core/         # Module, IO, and menu models and managers
├─ TrackLab.UI/           # Shared controls, converters, icons, and themes
├─ TrackLab.Simulator/    # Equipment simulator project (currently a scaffold)
└─ TrackLab.sln           # Visual Studio solution
```

### Project Dependencies

```text
TrackLab.Client ──────┬──> TrackLab.Core
                      └──> TrackLab.UI ───> TrackLab.Core

TrackLab.Simulator ───┬──> TrackLab.Core
                      └──> TrackLab.UI
```

## Requirements

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 with the **.NET desktop development** workload (recommended)

## Getting Started

### 1. Clone the repository

```powershell
git clone https://github.com/NobiNobita-GC/TrackLab.git
cd TrackLab
```

### 2. Restore and build

```powershell
dotnet restore TrackLab.sln
dotnet build TrackLab.sln
```

### 3. Run the client

```powershell
dotnet run --project .\TrackLab.Client\TrackLab.Client.csproj
```

Alternatively, open `TrackLab.sln` in Visual Studio, set `TrackLab.Client` as the startup project, and run it.

## Configuration

### Menu Configuration

The client menu is defined in:

```text
TrackLab.Client/Config/MenuConfig.json
```

Each menu item that opens a page uses `ViewModelType` to reference the fully qualified ViewModel type in the client assembly:

```json
{
  "Name": "Status",
  "ViewModelType": "TrackLab.Client.View.StatusViewModel"
}
```

To add a page:

1. Create matching `XxxView.xaml` and `XxxViewModel.cs` files under `TrackLab.Client/View`.
2. Make sure the ViewModel can be created with a parameterless constructor.
3. Add its fully qualified ViewModel type name to `MenuConfig.json`.

The menu configuration is copied to the output `Config` directory during the build.

### Equipment Layout Configuration

The equipment layout shown on the status page is defined in:

```text
TrackLab.Client/Config/LayoutConfig.xaml
```

The layout uses a standard WPF `Grid` to describe the positions of load ports, the transfer area, and process modules. At runtime, module-name placeholders such as `LP01`, `ROBOT01`, and `HP01` are replaced with `ModuleControl` instances bound to the matching module data.

When editing the layout:

1. The text of a placeholder `TextBlock` must exactly match a module name registered in `ModuleManager`.
2. A placeholder without a matching module remains a regular text element.
3. `LayoutConfig.xaml` is copied to the output directory as a content file, so rebuild the project or synchronize the output file after making changes.
4. Clicking a matched module opens a detail dialog that displays its name, index, type, and state.

### Demo Data

Demo modules and IO points are created by `InitializeRuntimeData()` in `TrackLab.Client/Bootstrapper.cs`:

- Hot plates: `HP01`–`HP10`
- Cool plates: `CP01`–`CP06`
- Load ports: `LP01`–`LP04`
- Transfer robot: `ROBOT01`
- Example DI/DO points: WaferPresent, VacuumOK, VacuumValve, HeaterOn, and others

The data exists only in process memory and returns to its initial state whenever the application restarts.

## Development Guidelines

- Keep domain models and business state in `TrackLab.Core`.
- Keep reusable controls, themes, icons, and converters in `TrackLab.UI`.
- Keep application pages and application-level interactions in `TrackLab.Client`.
- Follow Caliburn.Micro naming conventions when pairing Views and ViewModels.
- Before committing changes, run at least:

  ```powershell
  dotnet build TrackLab.sln --configuration Debug
  ```

## Current Status

- `TrackLab.Client`: runnable application prototype with a configurable equipment layout and module detail dialogs
- `TrackLab.Core`: basic hot plate, cool plate, load port, robot, IO, and menu management implementations
- `TrackLab.UI`: shared module control, status converters, vector icons, and themes
- `TrackLab.Simulator`: basic window scaffold; equipment simulation is not implemented yet
- Automated tests: no test project has been added yet

## Roadmap

- Integrate real equipment communication or a unified equipment abstraction layer
- Move startup demo data into configuration or persistent storage
- Implement data exchange between the Simulator and Client applications
- Add logging, error handling, and runtime diagnostics
- Add unit tests for the Core project
