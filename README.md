# capture-screen-macos-csharp

This is an example of creating a screenshot of the screen in macOS using C# in .NET 7.0.
I Couldn't find any fully working recent example of this online, so I thought I'd share it.

The example include a simple console application that takes a screenshot and saves it to a file.

NOTE: this is by no means a production ready example, nor is it necessarily the best way to do it,
This is just something that worked for me after looking for a solution for a while.

## Development

### Prerequisites

- .NET 7.0 SDK
- macOS 11.0 SDK
- Xcode 13.0

### Build

First time, make sure to run the following command:
```bash
dotnet workload install macos-11.0
```

Then, to build the project, run the following command:
```bash
dotnet build
```


## Running

To run the example, run the following command:
```bash
dotnet run -- <mode> <outputPath>
```

Where `<mode>` is either `active` or `primary` and `<outputPath>` is the path to the output file.
This is an example of a screenshot of the screen or window depending on the `captureMode` parameter.

- `active` - capture the active window.
- `primary` - capture the primary screen.




