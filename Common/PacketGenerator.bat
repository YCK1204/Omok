rem ���� �������� ���ϵ� ����
set CLIENT_SCRIPT_PATH=..\Omok\Assets\Scripts
set SERVER_SCRIPT_PATH=..\OmokServer\OmokServer

set ROOT_FBS=fbs/Protocol.fbs

rem ȯ�溯�� ���� ����
setlocal enabledelayedexpansion

if not exist "%CLIENT_SCRIPT_PATH%\Packet" (
    mkdir "%CLIENT_SCRIPT_PATH%\Packet"
)

if not exist "%CLIENT_SCRIPT_PATH%\FlatBuffers" (
    mkdir "%CLIENT_SCRIPT_PATH%\FlatBuffers"
)

if not exist "%SERVER_SCRIPT_PATH%\FlatBuffers" (
    mkdir "%SERVER_SCRIPT_PATH%\FlatBuffers"
)

del /S /Q "*.cs"
del /S /Q "%CLIENT_SCRIPT_PATH%\FlatBuffers\PKT*
del /S /Q "%SERVER_SCRIPT_PATH%\FlatBuffers\PKT*

set "fileList="

rem ���� ���丮�� ��� .fbs ������ ����
for %%f in (./fbs/*.fbs) do (
    set "fileList=!fileList! fbs/%%f"
)

rem flatc ���� (������ ���� ����Ʈ�� �� ���� ����)
START ./flatc.exe --csharp %fileList%
START ../OmokServer/PacketGenerator/bin/Debug/net8.0/PacketGenerator.exe %ROOT_FBS%

rem 3�� ��ٸ� �� ���� ����
timeout /t 3 /nobreak >nul
for %%f in (*.cs) do (
    copy "%%f" "%CLIENT_SCRIPT_PATH%/FlatBuffers"
    copy "%%f" "%SERVER_SCRIPT_PATH%/FlatBuffers"
)

rem ---------- Server -----------
XCOPY /Y ".\Server\PacketManager.cs" "%SERVER_SCRIPT_PATH%\Packet"
rem -----------------------------

rem ---------- Client -------------
XCOPY /Y ".\Client\PacketManager.cs" "%CLIENT_SCRIPT_PATH%\Packet"
rem -------------------------------

pause