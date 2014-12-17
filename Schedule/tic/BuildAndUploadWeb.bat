call app_info_setup.bat
REM get our ftp logon info
call d:\projects\SetFTPLogonInfoTanked.bat

:Actually do the unity build
rmdir build\web /S /Q
mkdir build\web

echo Building project...
%UNITY_EXE% -quit -batchmode -buildWebPlayer build/web .
echo Finished building.


rmdir temp /S /Q
mkdir temp
mkdir temp\%FILENAME%

xcopy build\web temp\%FILENAME%\ /E /F /Y

rename temp\%FILENAME%\web.html index.html

if not exist temp\%FILENAME%\index.html beeper.exe /p
if not exist temp\%FILENAME%\web.unity3d beeper.exe /p

ncftpput -u %_FTP_USER_% -p %_FTP_PASS_% -R %_FTP_SITE_% /www/ temp\*

echo File uploaded:  http://www.%_FTP_SITE_%/%FILENAME%

:Let's go ahead an open a browser to test it
start http://www.%_FTP_SITE_%/%FILENAME%
pause
