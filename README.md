# GsyncSwitch

## Changes
* Replaces Switch with separate ON and OFF options
* Removing screen related options
* Add shortcut for ON/OFF

The `HDR ON/OFF` is implemented by `HDRController.dll`, which is extracted from [AutoActions](https://github.com/Codectory/AutoActions).

<img width="182" alt="eRo9Z4eaV4" src="https://user-images.githubusercontent.com/20685540/202513175-ff30f8f2-1fa0-4a87-88d0-ebe3c7d89680.png">

---

Simple Windows App to switch G-Sync or HDR on/off with one click in taskbar

If you take the release (here on the right), put the 2 files in the same folder and launch GsyncSitch.exe for toolbar app or GsyncSitchEXE.exe to switch directly Gsync mode

To switch Gsync mode, just double click on icon in taskbar :
![image](https://user-images.githubusercontent.com/71530061/163377488-4f60ebdc-3005-47ec-89d9-f47d475a3db5.png)

Or use right click for menu :
![image](https://user-images.githubusercontent.com/71530061/163563377-569ec630-a67e-4d23-9330-11b757626d89.png)

----------------------------------------------------------------------------------------------------------------------------                                                                                                              
If you want to build the projects yourself :

- GsyncSwitchEXE : C++ project to make an exe to switch Gsync (using NVAPI)
- GsyncSwitch : C# project for the simple app in taskbar (it has GsyncSwitchEXE.exe in it)

To compile GsyncSwitchEXE project, you need NVAPI (not included for copyright purpose) available here :
https://developer.nvidia.com/nvapi


V1.1 : added HDR switch (at least for W11, not sure if shortcut win+alt+b works in W10 now : app just send shortcut)

v1.3 : added creator info so W11 antivirus doesn't get crazy for no reason + personal needs in bar :

v1.4 : added quick shortcut to sound control in menu

v2.0 : migrated app to .NET6.0, cause previous version couldn't access advanced sound settings
release also contains .NET6.0 framework, so there are more files, the one to launch is still GsyncSwitch.exe
<br>.NET Desktop Runtime 6.0.10 needed :
https://dotnet.microsoft.com/en-us/download/dotnet/6.0 

also added sound control info :
![Capture d’écran 2022-06-08 094926](https://user-images.githubusercontent.com/71530061/172562388-3d66311c-6547-4a5b-bbd0-5d260276441b.png)

