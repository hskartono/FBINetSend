# FBINetSend
3DS FBI Network Install tools

A simple library to be used as URL pusher for 3DS FBI Remote Installation.

First release, now you can pass parameter to the executable file.
This application support send multiple files or ony one file to your 3DS.

Syntax as follows:

Sending only one file to your 3DS FBI:

FBINetSend <3ds-ip-address> <http://base_url/to/cia/folder> <local/path/to/cia/file.cia>

For Example:

FBINetSend 192.168.2.24 http://192.168.2.23/3ds/cps1/ c:\inetpub\wwwroot\3ds\cps1\cadillac_and_dinosaurs.cia

The application will connect to your 3DS in 192.168.2.24 ip address, and sending URL to download http://192.168.2.23/3ds/cps1/cadillac_and_dinosaurs.cia

For multiple files:

FBINetSend 192.168.2.24 http://192.168.2.23/3ds/cps1/ c:\inetpub\wwwroot\3ds\cps1\

The application will scan c:\inetpub\wwwroot\3ds\cps1\ folder first, and then building list of URL based on your input (http://192.168.2.23/3ds/cps1/).

Be aware that all folder and url information should be trailing with slash or back-slash.

There is also step-by-step configuration, just pass nothing to the FBINetSend.exe, and the application will try asking your question about 3ds ip address, your base url and your cia path / filename location.
