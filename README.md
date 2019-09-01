HandyEditor
===

This repository contains a collection of handy `Editor` extensions for Unity.

To install it, download and unzip this repository or alternatively clone it. 
Then, from Unity's package manager, import the package:

![package manager screenshot](https://raw.githubusercontent.com/lordscales91/HandyEditor/master/Docs/Images/add_package.png)

Features
---

* Allows you to create custom C# Scripts. You can customize the namespace, the class name and the parent class.
* You can also create a `ScriptTemplate` asset, and pass it to the Custom C# Wizard. The options specified in the template will override the ones shown in the wizard. You can create one for your project, so that you don't need to specify the namespace every time.
* When creating a file in a folder outside `Assets/` (for example, when creating a script in a package). It will automatically create an ASMDEF file if there is none present. The name used will be the namespace in lowercase,
* It also allows you to create empty Markdown (.md), JSON (.json) and plain text files (.txt)

Usage
---
The options can be found unde the create menu on the project view:

![handy editor menu](https://raw.githubusercontent.com/lordscales91/HandyEditor/master/Docs/Images/menu.png)