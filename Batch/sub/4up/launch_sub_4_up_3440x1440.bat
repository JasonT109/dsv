START dsv.exe -id subPilotLeft -network-role server -screen-position-x 0 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 1720 -screen-height 720 -screen-mode 21x9c
ping 127.0.0.1 -n 4 > nul

START dsv.exe -id subPilotMid -screen-position-x 1720 -screen-position-y 0 -screen-fullscreen 0 -popupwindow -screen-width 1720 -screen-height 720 -screen-mode 21x9c
ping 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -screen-position-x 0 -screen-position-y 720 -screen-fullscreen 0 -popupwindow -screen-width 1720 -screen-height 720 -screen-mode 21x9c
ping 127.0.0.1 -n 2 > nul

START dsv.exe -id subPilotRight -screen-initial vitals -screen-position-x 1720 -screen-position-y 720 -screen-fullscreen 0 -popupwindow -screen-width 1720 -screen-height 720 -screen-mode 21x9c
 