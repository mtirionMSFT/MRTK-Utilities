# MRTK Utilities

This is the Unity based HoloLens 2 Application using MRTK 2. This application used to demonstrate MRTK Utilities and basic setup of a project.

## Prerequisites

To work with this repo you need these tools:

* Windows 10 or Windows 11 (make sure you update to the latest version)
* [Visual Studio 2019 or 2022](https://visualstudio.microsoft.com/vs/)
  Make sure to select *Universal Windows Platform development*,  *Game development with Unity* and *Game development with C++*.
* [Unity 2020.3 LTS](https://unity3d.com/get-unity/download)
  Make sure to select *Universal Windows Platform Build Support, Windows Build Support (IL2CPP)* to develop for HoloLens.
* [Microsoft Mixed Reality Toolkit (MRTK) v2.8](https://github.com/microsoft/MixedRealityToolkit-Unity/releases/latest)
  The solution contains the correct MRTK version in the repo. Configuration is done using the [Mixed Reality Feature Tool](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool).

## Setup

When you retrieve this project for the first time from the repo, set it up with these steps:

1. Open the project with **Unity 2020.3 (LTS)**

2. In the Build Settings (menu open File > Build Settings ...)
3. Under Platform select **Universal Windows Platform**
4. Use the settings as shown in the image below:
![Unity Build Settings](unity-build-settings.png)
5. Click **Switch Platform**
6. In the **Project** pane under **Assets\Scenes** double click **Main** to open the main scene.

## Build

To build the application, execute the **Build** in the Build Settings dialog. A folder picker dialog will open. Create a **Build** folder or select the existing one. After the build is done, a Windows Explorer window will open in the Build folder.

A Visual Studio solution is generated from the Unity project. Open the solution (.sln) with Visual Studio. In the top menu bar of Visual Studio select **Release** and **ARM64** for HoloLens deployment.

![Visual Studio settings](visual-studio-settings.png)

You can deploy to **Device** when attached to USB or to **Remote** over WIFI. If you want to deploy over WIFI, make sure you have set up the **Machine Name** in the Debugging settings. Right-click the (Universal Windows) project and click **Properties**. Under *Configuration Properties* select **Debugging**. When **Remote Machine** is selected under *Debugger to launch* you'll see this UI. Put the IP address of your HoloLens in the **Machine Name**.

![Visual Studio Remote settings](visual-studio-remote-settings.png)

For more information (including setting up your HoloLens for side loading) see [Using Visual Studio to deploy and debug - Mixed Reality | Microsoft Docs](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2)

