installPath=Session.Property("CustomActionData")

Set WShShell = CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")
frameworkPath="c:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319"

if fso.FolderExists(frameworkPath) = False then
    frameworkPath="c:\WINDOWS\Microsoft.NET\Framework\v4.0.30319"
end if

strCommand = """" & Replace(installPath, """", "") & "DrawShip.Viewer.exe"" /mode:install"
WshShell.Run strCommand, True, 1

'' Permit route via the firewall
WshShell.Run "netsh advfirewall firewall add rule name=""DrawShip viewer"" dir=in action=allow protocol=TCP localport=5142", True, 1
