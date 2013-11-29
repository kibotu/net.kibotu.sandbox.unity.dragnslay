@echo off

set PATH=%~dp0msys\bin;%~dp0win-rsync;%PATH%

sh copy_script.sh WIN unity_export dragnslay-android/unity_export/dragnslay/assets dragnslay-android/dragnslay/assets
sh copy_script.sh WIN unity_export dragnslay-android/unity_export/dragnslay/libs dragnslay-android/dragnslay/libs