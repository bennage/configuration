"Adding the [BetterConfig.dll] assembly to the script..." 
$scriptPath = Split-Path (Get-Variable MyInvocation -Scope 0).Value.MyCommand.Path
$assembly = Get-ChildItem $scriptPath -Include "BetterConfig.dll" -Recurse

Add-Type -Path $assembly.FullName
"Added " + $assembly.FullName

[BetterConfig.LocalHelper]::Test("myTest")