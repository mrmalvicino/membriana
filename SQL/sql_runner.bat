@echo off
setlocal enabledelayedexpansion

cls
echo ----------
echo SQL RUNNER
echo ----------
echo.
echo This program automates the execution of SQL scripts.
echo.
echo Press any key to continue...
pause >nul

:menu
cls
echo ----------
echo SQL RUNNER
echo ----------
echo.
echo Choose database host:
echo.
echo 1- Default localhost
echo 2- Local server with integrated security
echo 3- External server using credentials
echo.
echo Enter your choice (1, 2 or 3):

set /p choice=

if "%choice%"=="1" (
    set database_host=localhost\SQLEXPRESS
) else if "%choice%"=="2" (
    echo Enter custom SQL Express path:
    set /p local_path=
    set database_host=!local_path!\SQLEXPRESS
) else if "%choice%"=="3" (
    echo Enter IP or web address:
    set /p server_ip_address=
    echo Enter database username:
    set /p server_username=
    echo Enter database password:
    set /p server_password=
    set database_host=!server_ip_address! -U !server_username! -P !server_password!
) else (
    echo Invalid option. Please try again.
    goto menu
)

:submenu
cls
echo ----------
echo SQL RUNNER
echo ----------
echo.
echo The selected host data is: !database_host!
echo.
echo 1- Insert initial data
echo 2- Insert dummy data
echo 3- Run all scripts
echo 4- Back to host menu
echo.

set /p selection=

if "%selection%"=="1" (
    sqlcmd -S !database_host! -i insert_initial_data.sql -f 65001
    echo.
) else if "%selection%"=="2" (
    sqlcmd -S !database_host! -i insert_dummy_data.sql -f 65001
    echo.
) else if "%selection%"=="3" (
    sqlcmd -S !database_host! -i insert_initial_data.sql -f 65001
    echo.
    sqlcmd -S !database_host! -i insert_dummy_data.sql -f 65001
    echo.
) else if "%selection%"=="4" (
    echo Cancelled by user.
    pause
    goto menu
) else (
    echo Invalid option. Please try again.
    goto submenu
)

pause