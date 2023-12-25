# MVCFrame

MVCFrame is a project that simulates a computer system with various components such as a CPU, external devices, memory, and more. It uses the Model-View-Controller (MVC) design pattern.

## Components

The main components of the system are:

- System Clock
- Central Processing Unit (CPU)
- External Devices
- Operational Memory (RAM)
- Process ID Generator
- Ready Queue for processes
- Device Queue for processes
- CPU Scheduler
- Memory Manager
- Device Scheduler
- Process Generator (Random Number Generator)
- Model Settings
- Statistics

Each component is represented as a class in the codebase.

## Model

The `Model` class ([MVCFrame/Model.cs](MVCFrame/Model.cs)) is the heart of the system. It contains instances of all the components and methods for their interaction. The `WorkingCycle` method simulates a working cycle of the computer system.

## View

The `ViewDetailed` class ([MVCFrame/ViewDetailed.cs](MVCFrame/ViewDetailed.cs)) is responsible for displaying the state of the model to the user. It uses data binding to automatically update the view when the model changes.

## Usage

To use this project, you need to create an instance of the `Model` class and then create an instance of the `ViewDetailed` class, passing the model instance to it. Then, you can call the `WorkingCycle` method on the model instance to start the simulation.

## Libraries

This project uses the `Queues.dll` and `Structures.dll` libraries located in the `Lib/` directory.

## Building

To build this project, open `MVCFrame.sln` in Visual Studio and build the solution.
