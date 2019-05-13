@echo off
set installDir=%ProgramFiles%\DrawShip
set source=%~dp0
set netframework32=c:\Windows\Microsoft.NET\Framework64\v4.0.30319\
set netframework64=c:\Windows\Microsoft.NET\Framework\v4.0.30319\

if not exist %installDir% mkdir "%installDir%" 2> nul

if not exist "%installDir%" (
	set installDir=%SystemDrive%\DrawShip
	mkdir "%installDir%" 2> nul
)

echo Installing to %installDir%

set installDirEscaped=%installDir:\=\\%

xcopy "%source%*.*" "%installDir%\" /Y /h
"%installDir%\DrawShip.Viewer.exe" /mode:install

echo Windows Registry Editor Version 5.00 > "%installDir%\register_manually.reg"
echo. >> "%installDir%\register_manually.reg"
echo [HKEY_CLASSES_ROOT\xmlfile\shell\Preview in DrawShip] >> "%installDir%\register_manually.reg"
echo. >> "%installDir%\register_manually.reg"
echo [HKEY_CLASSES_ROOT\xmlfile\shell\Preview in DrawShip\command] >> "%installDir%\register_manually.reg"
echo @="\"%installDirEscaped%\\DrawShip.Viewer.exe\" \"%%1\"" >> "%installDir%\register_manually.reg"
echo. >> "%installDir%\register_manually.reg"
echo @REM [HKEY_CLASSES_ROOT\xmlfile\shell\Preview in DrawShip (image)] >> "%installDir%\register_manually.reg"
echo. >> "%installDir%\register_manually.reg"
echo [HKEY_CLASSES_ROOT\xmlfile\shell\Preview in DrawShip (image)\command] >> "%installDir%\register_manually.reg"
echo @="\"%installDirEscaped%\\DrawShip.Viewer.exe\" \"%%1\" /format:Image" >> "%installDir%\register_manually.reg"
echo. >> "%installDir%\register_manually.reg"

if exist "%netframework32%regasm.exe" (
	call "%netframework32%regasm.exe" "%installDir%\PreviewIo.dll" /codebase /nologo /silent
)
if exist "%netframework64%regasm.exe" (
	call "%netframework64%regasm.exe" "%installDir%\PreviewIo.dll" /codebase /nologo /silent
)

if "%errorlevel%"=="0" echo Installed successfully

pause