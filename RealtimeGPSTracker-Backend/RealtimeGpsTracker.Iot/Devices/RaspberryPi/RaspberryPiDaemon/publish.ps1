function FileTransferProgress {
param($e)
    Write-Progress `
        -Activity "Uploading" -Status ("{0:P0} complete:" -f $e.OverallProgress) `
        -PercentComplete ($e.OverallProgress * 100)
    Write-Progress `
        -Id 1 -Activity $e.FileName -Status ("{0:P0} complete:" -f $e.FileProgress) `
        -PercentComplete ($e.FileProgress * 100)
}

# Set name of the project
$projectName = "RaspberryPiDaemon"

# Read last timestamp of previous run, if any
$lastTimestamp = $Null
$timestampFile = "publish_timestamp.dat"
if (Test-Path $timestampFile)
{
    $timestampFileContent = (Get-Content $timestampFile);

    if (-Not ([string]::IsNullOrEmpty($timestampFileContent)))
    {
        $lastTimestamp = [DateTime]::ParseExact($timestampFileContent, "yyyy-MM-dd HH:mm:ss", $Null)
	}    
}
else
{
    $lastTimestamp = $Null
}

if (-Not ([string]::IsNullOrEmpty($lastTimestamp)))
{
    Write-Host ("Last publish: " + $lastTimestamp.ToString("yyyy-MM-dd HH:mm:ss"))
}

# Load WinSCP .NET assembly
Add-Type -Path "$PSScriptRoot\WinSCPnet.dll"

# Set up session options
$sessionOptions = New-Object WinSCP.SessionOptions -Property @{
    Protocol = [WinSCP.Protocol]::Sftp
    HostName = "raspberrypi.local"
    UserName = "pi"
    Password = "Yg9wKwsAgkKy6oqi-is-Jw"
    SshHostKeyFingerprint = "ssh-ed25519 255 n9/wNn1ddP10WpYcAUqQjFQkVoSnr3Zqm/UNiwTo2bc="
}

# Start session
$session = New-Object WinSCP.Session
try {
    # Will continuously report progress of transfer
    $session.add_FileTransferProgress( { FileTransferProgress($_) } )
    
    # Connect
    $session.Open($sessionOptions)

    # Check if process with project name is already running
    try {
        $session.ExecuteCommand("killall " + $projectName + "/" + $projectName).Check();
        Write-Host ("Killing previous " + $projectName + " run.");
    }
    catch {
        Write-Host ("Nothing to kill. " + $projectName + " is not running.");
    }

    Start-Process dotnet -ArgumentList 'publish -r linux-arm' -Wait -NoNewWindow -WorkingDirectory $PSScriptRoot
    
    $transferOptions = New-Object WinSCP.TransferOptions
    if ($Null -ne $lastTimestamp)
    {
        $transferOptions.FileMask = ("*>" + $lastTimestamp.ToString("yyyy-MM-dd HH:mm:ss"))
    }
    else
    {
        $transferOptions.FileMask = "*>=today"
    }    
    
    $result = $session.PutFiles("$PSScriptRoot\bin\Debug\netcoreapp3.1\linux-arm\publish\*", $projectName + "/", $False, $transferOptions).Check();
    Write-Host $result

    # Persist timestamp for the next run
    $lastTimestamp = (Get-Date -Format "yyyy-MM-dd HH:mm:ss")
    Set-Content -Path $timestampFile -Value $lastTimestamp

    $session.ExecuteCommand("chown pi /home/pi/" + $projectName + " -R").Check();
    $session.ExecuteCommand("chmod 777 /home/pi/" + $projectName + " -R").Check();

    Write-Host ("You can now attach a debugger to the running " + $projectName + " process ... (Use CTRL+ALT+P or SHIFT+ALT+P in VS 2019.)");
    $session.ExecuteCommand($projectName + "/" + $projectName);
}
finally {
    $session.Dispose()
}