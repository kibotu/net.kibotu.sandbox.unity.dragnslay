@echo off
set currentDir=%~dp0
set val1=D:\Libraries\android\adt\sdk\platform-tools
SetX PATH "%val1%" /m
echo -----------------------------
echo -- installed in global vars
echo -----------------------------
echo adb=%currentDir%
PAUSE