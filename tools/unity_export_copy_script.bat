@echo off

set PATH=%~dp0msys\bin;%~dp0win-rsync;%PATH%

sh copy_script.sh WIN unity_export android/unity_export/dragnslay/assets android/DragnSlay/assets
sh copy_script.sh WIN unity_export android/unity_export/dragnslay/libs android/DragnSlay/libs