@echo off
set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

rem set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
rem echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)


set VERSIONFILE=WASD.version
rem The following requires the JQ program, available here: https://stedolan.github.io/jq/download/
c:\local\jq-win64  ".VERSION.MAJOR" %VERSIONFILE% >tmpfile
set /P major=<tmpfile

c:\local\jq-win64  ".VERSION.MINOR"  %VERSIONFILE% >tmpfile
set /P minor=<tmpfile

c:\local\jq-win64  ".VERSION.PATCH"  %VERSIONFILE% >tmpfile
set /P patch=<tmpfile

c:\local\jq-win64  ".VERSION.BUILD"  %VERSIONFILE% >tmpfile
set /P build=<tmpfile
del tmpfile
set VERSION=%major%.%minor%.%patch%
if "%build%" NEQ "0"  set VERSION=%VERSION%.%build%

echo %VERSION%


set d=GameData
if exist %d% goto one
mkdir %d%
:one
set d=Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=GameData\WasdEditorCamera
if exist %d% goto three
mkdir %d%
:three
set d=GameData\WasdEditorCamera\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=GameData\WasdEditorCamera\PluginData
if exist %d% goto five
mkdir %d%
:five
set d=GameData\WasdEditorCamera\Textures
if exist %d% goto six
mkdir %d%
:six

rem del /y \Gamedata\WasdEditorCamera\Textures\*.*

xcopy src\Textures\WASD*.png   GameData\WasdEditorCamera\Textures /Y
copy bin\Release\WasdEditorCamera.dll Gamedata\WasdEditorCamera\Plugins
copy  WASD.version Gamedata\WasdEditorCamera
copy README.md WasdEditorCamera
copy ChangeLog.txt Gamedata\WasdEditorCamera
copy WASD_Settings.cfg Gamedata\WasdEditorCamera\PluginData
rem copy WASD_Settings.cfg.default %HOMEDIR%\install\Gamedata\WasdEditorCamera\PluginData
copy ..\MiniAVC.dll  Gamedata\WasdEditorCamera



set FILE="%RELEASEDIR%\WasdEditorCamera-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\WasdEditorCamera
