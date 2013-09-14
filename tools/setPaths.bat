@echo off
set currentDir=%~dp0
set val1=D:\Libraries\android\adt\sdk\platform-tools
set val2=C:\Program Files (x86)\Git\bin
SetX PATH "%val1%" /m
SetX PATH "%val2%" /m
echo -- installed in global vars
echo -----------------------------
echo adb=%val1%
echo git=%val2%
PAUSE