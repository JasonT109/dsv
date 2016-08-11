START dsv.exe -id subPilotLeft -network-role none -screen-position-x 0 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 4 > nul

START dsv.exe -id subPilotMid -network-role none -screen-position-x 960 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -network-role none -screen-position-x 0 -screen-position-y 540 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -network-role none -screen-initial vitals -screen-position-x 960 -screen-position-y 540 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
