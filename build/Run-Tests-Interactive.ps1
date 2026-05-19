Param(
    [Parameter(Mandatory = $true)]
    [string] $SiteUrl,

    [Parameter(Mandatory = $true)]
    [string] $ClientId,

    [Parameter(Mandatory = $false)]
    [string] $Filter = "FullyQualifiedName~GetPnPHubSiteTest",

    [Parameter(Mandatory = $false)]
    [switch] $Blame,

    [Parameter(Mandatory = $false)]
    [switch] $LocalPnPFramework
)

$env:PnPTests_SiteUrl = $SiteUrl
$env:PnPTests_ClientId = $ClientId
$env:PnPTests_CredentialManagerLabel = $null
$env:PnPTests_Username = $null
$env:PnPTests_Password = $null

$currentTelemetrySetting = $env:PNPPOWERSHELL_DISABLETELEMETRY
$env:PNPPOWERSHELL_DISABLETELEMETRY = $true

$testCsproj = Join-Path $PSScriptRoot "..\src\Tests\PnP.PowerShell.Tests.csproj"
$testCmd = "dotnet test `"$testCsproj`" --filter `"$Filter`""
if ($Blame)        { $testCmd += " --blame" }
if ($LocalPnPFramework) { $testCmd += " -p:LocalPnPFramework=true" }

Write-Host "Executing $testCmd" -ForegroundColor Yellow
Write-Host "A browser window will open for Interactive sign-in. Use the account that has SharePoint Admin on the tenant." -ForegroundColor Cyan

Invoke-Expression $testCmd

$env:PnPTests_SiteUrl = $null
$env:PnPTests_ClientId = $null
$env:PNPPOWERSHELL_DISABLETELEMETRY = $currentTelemetrySetting
