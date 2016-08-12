START dsv.exe -id subPilotLeft -network-role server -screen-position-x 0 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 4 > nul

START dsv.exe -id subPilotMid -screen-position-x 960 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -screen-position-x 0 -screen-position-y 540 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
PING 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -screen-initial vitals -screen-position-x 960 -screen-position-y 540 -screen-fullscreen 0 -popupwindow -screen-width 960 -screen-height 540
