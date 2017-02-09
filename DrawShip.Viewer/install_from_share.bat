@echo off
set installDir=%ProgramFiles%\DrawShip
@set source=%~dp0

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

if "%errorlevel%"=="0" @echo Installed successfully

pause