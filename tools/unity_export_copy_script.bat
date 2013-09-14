@echo off

set PATH=%~dp0msys\bin;%~dp0win-rsync;%PATH%

sh copy_script.sh WIN unity_export dragnslay-android/unity_export/dragnslay dragnslay-android/dragnslay

PAUSE