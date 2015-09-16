@echo off
set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)

type WASD.version
set /p VERSION= "Enter version: "



set d=%HOMEDIR\install
if exist %d% goto one
mkdir %d%
:one
set d=%HOMEDIR%\install\Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=%HOMEDIR%\install\Gamedata\WasdEditorCamera
if exist %d% goto three
mkdir %d%
:three
set d=%HOMEDIR%\install\Gamedata\WasdEditorCamera\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=%HOMEDIR%\install\Gamedata\WasdEditorCamera\Textures
if exist %d% goto five
mkdir %d%
:five
del %HOMEDIR%\install\Gamedata\WasdEditorCamera\Textures\*.*

xcopy src\Textures\WASD*.png   %HOMEDIR%\install\GameData\WasdEditorCamera\Textures /Y
copy bin\Release\WasdEditorCamera.dll %HOMEDIR%\install\Gamedata\WasdEditorCamera\Plugins
copy  WASD.version %HOMEDIR%\install\Gamedata\WasdEditorCamera
copy README.md %HOMEDIR%\install\Gamedata\WasdEditorCamera
copy ChangeLog.txt %HOMEDIR%\install\Gamedata\WasdEditorCamera
copy config.cfg %HOMEDIR%\install\Gamedata\WasdEditorCamera
copy WASD_Settings.cfg.default %HOMEDIR%\install\Gamedata\WasdEditorCamera
pause

%HOMEDRIVE%
cd %HOMEDIR%\install

set FILE="%RELEASEDIR%\WasdEditorCamera-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\WasdEditorCamera
