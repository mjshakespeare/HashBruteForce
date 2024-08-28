# PAN Hash Cracker CLI Tool

This tool is a simple command-line interface (CLI) application that attempts to crack a SHA-256 hashed PAN (Primary Account Number) using a known PAN prefix. The tool is intended for educational and demonstration purposes only.

## Prerequisites

Ensure you have the following installed on your system:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- A terminal or command prompt for running the CLI

## How to Use

### Build the Application

First, clone the repository and navigate to the project directory. Then, build the application using the following command:

```bash
dotnet build
```
## Running the Application

```bash
dotnet run -- --panprefix <PAN_PREFIX> --panlength <PAN_LENGTH> --targethash <TARGET_HASH>
```


* `--panprefix`: The known prefix of the PAN. This is a required parameter.
* `--panlength`: The total length of the PAN. This is optional and defaults to `14` if not provided.
* `--targethash`: The SHA-256 hash of the target PAN. This is a required parameter.

## Output
The application will attempt to find the PAN that matches the given hash. If successful, it will output the PAN to the console.

```plaintext
PAN found: 1234567890123456
```

If the PAN is not found, the tool will exit without producing output.

## Notes
This tool is for educational purposes only and should not be used for any illegal activities.
The process of brute-forcing a SHA-256 hash can be computationally intensive and time-consuming.
Ensure that you have permission to attempt cracking a hash before using this tool.