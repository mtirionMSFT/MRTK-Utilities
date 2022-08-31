# MRTK Utilities

This is the Unity based HoloLens 2 Application using MRTK 2. This application used to demonstrate MRTK Utilities and basic setup of a project. The implementation is based on learnings from a project a Microsoft CSE team I was part of did for an enterprise customer. There is a series of blogposts that explain more about the learnings contained in this repo:

An overview of the posts can be found here: [Learnings from developing a HoloLens Application - Martin Tirion - Medium](https://mtirion.medium.com/learnings-from-developing-a-hololens-application-c0c9d38730fc). The topics we covered in this sharing are:

- [Setting up a Unity-based application for HoloLens with MRTK | by Martin Tirion | Aug, 2022 | Medium](https://mtirion.medium.com/setting-up-a-unity-based-application-for-hololens-with-mrtk-6d079aff47a6)
- [Adding Application Settings to a Unity-based HoloLens Application | by Martin Tirion | Aug, 2022 | Medium](https://mtirion.medium.com/adding-application-settings-to-a-unity-based-hololens-application-c9632fb52c5a)
- [Authenticate a HoloLens Application to the Azure AD. | by Martin Tirion | Aug, 2022 | Medium](https://mtirion.medium.com/authenticate-a-hololens-application-to-the-azure-ad-ad551fbb8440)
- [A Ready To Use Debug Console for your HoloLens Application | by Martin Tirion | Aug, 2022 | Medium](https://mtirion.medium.com/a-ready-to-use-debug-console-for-you-hololens-application-6b9913d93377)
- [Creating a PDF Document Viewer in a HoloLens Application | by Martin Tirion | Aug, 2022 | Medium](https://mtirion.medium.com/creating-a-pdf-document-viewer-in-a-hololens-application-fad0f1399e27)

## Unity Packages

In the folder [CSE.MRTK.Toolkit.Packages](./CSE.MRTK.Toolkit.Packages) these Unity packages are made available:

* [Application base](./CSE.MRTK.Toolkit.Packages/cse.mrtk.toolkit.application.base.unitypackage) - This provides an **Application** hierarchy as starting point for the structure of an Unity application using MRTK for HoloLens. In contains some basic classes as well on setting, authentication and more. For more information see [Setting up a Unity-based application for HoloLens with MRTK ](https://mtirion.medium.com/setting-up-a-unity-based-application-for-hololens-with-mrtk-6d079aff47a6), [Adding Application Settings to a Unity-based HoloLens Application](https://mtirion.medium.com/adding-application-settings-to-a-unity-based-hololens-application-c9632fb52c5a) and [Authenticate a HoloLens Application to the Azure AD](https://mtirion.medium.com/authenticate-a-hololens-application-to-the-azure-ad-ad551fbb8440)
* [Debug Console](./CSE.MRTK.Toolkit.Packages/cse.mrtk.toolkit.debugconsole.unitypackage) - A **debug console** prefab using MRTK to show debug messages at runtime. You can also save the messages to a file. For more information see [A Ready To Use Debug Console for your HoloLens Application](https://mtirion.medium.com/a-ready-to-use-debug-console-for-you-hololens-application-6b9913d93377).
* [Document Viewer](./CSE.MRTK.Toolkit.Packages/cse.mrtk.toolkit.documentviewer.unitypackage) - A **document viewer** prefab using MRTK and Paroxe (not included) to show PDF documents. For more information see [Creating a PDF Document Viewer in a HoloLens Application](https://mtirion.medium.com/creating-a-pdf-document-viewer-in-a-hololens-application-fad0f1399e27).

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

## Package

For instructions how to package the app to deploy to a HoloLens, see the article [Creating the App Package to publish](./docs/publish.md).
