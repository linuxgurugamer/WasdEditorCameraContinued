rem line must not be used

set H=R:\KSP_1.1.4_dev
echo %H%

set d=%H%
if exist %d% goto one
mkdir %d%
:one
set d=%H%\Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=%H%\Gamedata\WasdEditorCamera
if exist %d% goto three
mkdir %d%
:three
set d=%H%\Gamedata\WasdEditorCamera\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=%H%\Gamedata\WasdEditorCamera\Textures
if exist %d% goto five
mkdir %d%
:five



xcopy bin\Debug\WasdEditorCamera.dll  %H%\GameData\WasdEditorCamera\Plugins\  /Y
xcopy src\Textures\*  %H%\GameData\WasdEditorCamera\Textures /Y
xcopy WASD_Settings.cfg %H%\GameData\WasdEditorCamera /y
xcopy WASD_Settings.cfg.default %H%\GameData\WasdEditorCamera /y
