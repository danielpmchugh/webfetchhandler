# Web Fetch Handler Service

This is a console application that retrieves data from an API and saves it to a JSON file.

## Installation

To install the application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution file in Visual Studio.
3. Run `dotnet build` to build the application.
4. Run `dotnet run <config-name>` to run with the configuration file appsettings.<config-name>.json 

## Usage

To use the application, run the executable file from the command line. The application will retrieve data from the API for the symbols specified in the configuration file, and save the results to a JSON file in the directory specified in the configuration file.

## Configuration

The application uses an `appsettings.json` file to configure its settings. You can modify this file to change the application's behavior.

The following settings are available:

- `URL`: The URL to fetch data from. You can modify this to fetch data from a different API endpoint.
- `BackoffTime`: The amount of time to wait between fetches. You can modify this to change the fetch frequency.
- `FileSinkDir`: The directory to save fetched data to. You can modify this to change the save location.
- `Symbols`: A list of symbols to fetch data for. You can modify this to fetch data for different symbols.

## Contributing

If you would like to contribute to the project, please follow these steps:

1. Fork the repository to your own GitHub account.
2. Create a new branch for your changes.
3. Make your changes and commit them to your branch.
4. Push your branch to your GitHub account.
5. Create a pull request from your branch to the main repository.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.