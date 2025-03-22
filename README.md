```markdown
# PHI Redaction Application

This repository contains a .NET 8 solution that processes simulated lab order documents, identifies and redacts Protected Health Information (PHI), and outputs the sanitized data. The solution consists of two projects:
1. **Class Library**: Contains the core logic for PHI identification and redaction.
2. **MVC Application**: Provides a user interface for uploading text files, processing them, and displaying the sanitized output.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Building the Application](#building-the-application)
- [Running the Application](#running-the-application)
- [Hosting on IIS (Windows)](#hosting-on-iis-windows)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

---

## Prerequisites

Before building and running the application, ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (optional, for easier development)
- [IIS (Internet Information Services)](https://learn.microsoft.com/en-us/iis/install/installing-iis-7/installing-iis-on-windows-vista-and-windows-7) (if hosting on Windows)

---

## Building the Application

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the Solution**:
   ```bash
   dotnet build
   ```

---

## Running the Application

1. **Run the MVC Application**:
   Navigate to the MVC project directory and start the application:
   ```bash
   cd YourMvcProjectFolder
   dotnet run
   ```

2. **Access the Application**:
   Open your browser and navigate to `https://localhost:7119`. The homepage will display a form for uploading text files.

---

## Hosting on IIS (Windows)

Follow these steps to host the application on IIS:

### Step 1: Install IIS
1. Open the **Control Panel** and go to **Programs > Turn Windows features on or off**.
2. Enable **Internet Information Services** and its required components (e.g., ASP.NET, .NET Extensibility).
3. Click **OK** and wait for the installation to complete.

### Step 2: Publish the Application
1. Navigate to the solution directory and publish the MVC project:
   ```bash
   cd YourMvcProjectFolder
   dotnet publish --configuration Release --output ./publish
   ```
2. The published files will be located in the `./publish` folder.

### Step 3: Configure IIS
1. Open **IIS Manager**.
2. Right-click on **Sites** and select **Add Website**.
3. Provide a site name, set the physical path to the `./publish` folder, and configure the binding (e.g., `http://localhost:80`).
4. Click **OK**.

### Step 4: Grant Permissions
1. Ensure the IIS application pool identity has read and execute permissions on the `./publish` folder.
2. Restart the IIS site.
3. Provide write & execute permission to IIS_IUSRS user for the wwwroot folder.

### Step 5: Access the Application
Open your browser and navigate to the configured URL (e.g., `http://localhost`).

---

## Usage

1. **Upload Files**:
   - On the homepage, use the form to upload one or more text files containing lab order documents.
   
2. **Process Files**:
   - Click the **Upload and Redact** button to process the files. The application will identify and redact PHI data.

3. **View Results**:
   - The sanitized data will be exported (zip file or a single file).

---

## Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Submit a pull request with a detailed description of your changes.

---

For any questions or issues, please open an issue on the repository. Thank you for using the PHI Redaction Application!
``` 
