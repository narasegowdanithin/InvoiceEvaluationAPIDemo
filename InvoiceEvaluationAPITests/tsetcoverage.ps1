# PURPOSE: Automates the running of  Tests and Genrate Code Coverage and Test Report 

# Install ReportGenerator globally if not already installed
# This only needs to be installed once (globally)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Install liquid globally if not already installed (to convert .trx to HTML)
dotnet tool install --global LiquidTestReports.Cli --version 2.0.0-beta.2
# Save the current directory into a variable
$dir = pwd

# Remove previous test run results, including subfolders with GUID names
Remove-Item -Recurse -Force "$dir/TestResults/" -ErrorAction SilentlyContinue

# Run the tests and collect code coverage using Coverlet.
# Replace YOURSOLUTIONNAME.sln with your actual solution name
$output = [string] (& dotnet test ../InvoiceEvaluationAPIDemo.sln --logger "trx;LogFileName=testresults.trx" `
    --results-directory "$dir/TestResults/" --collect:"XPlat Code Coverage" 2>&1)
Write-Host "Last Exit Code: $lastexitcode"
Write-Host $output

# Remove previous coverage reports if they exist
Remove-Item -Recurse -Force "$dir/CoverageReport/" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "$dir/TrxReport/" -ErrorAction SilentlyContinue

# Create a directory to store the coverage history (if it doesn't already exist)
if (!(Test-Path -path "$dir/CoverageHistory")) {
    New-Item -ItemType directory -Path "$dir/CoverageHistory"
}

if (!(Test-Path -path "$dir/TrxHistory")) {
    New-Item -ItemType directory -Path "$dir/TrxHistory"
}

# Generate the Code Coverage HTML Report using ReportGenerator
# Here, we're pointing to the location of the coverage reports
reportgenerator -reports:"$dir/**/coverage.cobertura.xml" -targetdir:"$dir/CoverageReport" -reporttypes:Html -historydir:"$dir/CoverageHistory" 

# Generate Test Result HTML Report from .trx file using liquid tool
# Here, we're pointing to the location of the .trx file (test result file)
$trxFilePath = Join-Path $dir "TestResults\*.trx"
$trxFile = Get-ChildItem -Path "$dir\TestResults" -Filter *.trx | Select-Object -First 1
# Create the output directory if it doesn't exist
    $outputDir = "$dir\TrxReport"
    if (!(Test-Path -Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir
    }
if (Test-Path $trxFilePath) {
    
    Copy-Item -Path $trxFilePath -Destination "$dir/TrxHistory" -Force
    #liquid --inputs "File=\TestResults\AA48F_2025-04-04_19_24_49.trx" --output-file "C:\Users\rashmi\source\repos\InvoiceEvaluationAPI\InvoiceEvaluationAPI.Tests\trxHistory\testresult.html"
    liquid --inputs "File=$($trxFile.FullName)" --output-file "$outputDir\testReport.html"
   # trx-to-html -i "$trxFilePath" -o "$dir/TrxReport" -p # Convert TRX to HTML
} else {
    Write-Host "No .trx file found, skipping test result report generation."
}

# Open the generated HTML reports (if running on a workstation)
$osInfo = Get-CimInstance -ClassName Win32_OperatingSystem
if ($osInfo.ProductType -eq 1) {
    # Open both code coverage and test result reports (both should be in the same directory)
    Start-Process "$dir/CoverageReport/index.html"
    Start-Process "$dir/TrxReport/testReport.html"
}

Write-Host "Process completed successfully."
