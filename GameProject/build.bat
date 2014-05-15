cd "D:\contextproject\Contextproject-Group5\GameProject"
start /w "" "C:\Program Files (x86)\Unity\Editor\Unity.exe" -batchmode -quit -nographics -projectPath %CD% -buildWindowsPlayer game.exe
if %errorlevel% neq 0 exit /b %errorlevel%
cd game_Data\Managed
"C:\Users\Pim\Downloads\NUnit-2.6.0.12051\bin\nunit-console.exe" Assembly-CSharp.dll
