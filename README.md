# VirginSales Blazor Application  
## Overview

It is a small Blazor Server application that lists sales from a CSV data store and visualizes the comparison between the sale cost and manufacturing cost by product.

![image](https://github.com/user-attachments/assets/469eaf22-6204-4471-83cc-b42dd2513846)

## Prerequisite & Dependency
- .Net 8 SDK
- Visual Studio 2022
- MudBlazor
- CsvParser
- NodaMoney

## Build and Run the App
- git clone the project
- Open the solution virginsales.sln
- Build (NuGet packages should be restored) and run

## Known Issues
- Implement Paging on the Grid - should have used DataGrid and only paged data should be fetched into memory.
- Error handling
- Logging
