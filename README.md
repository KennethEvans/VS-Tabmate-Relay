# Tabmate Relay

Celsys, the makers of Clip Studio Paint (CSP) also make a device called the Tabmate, which is a hand-held control with buttons, a wheel, and a control pad. The Tabmate makes it much easier to work with the program when you use it on a tablet, like the Surface Book, with the keyboard out of the way. The problem is that it only works with CSP. 
 (See https://www.clipstudio.net/en/ and https://www.clipstudio.net/promotion/tabmate/en/)

Tabmate Relay, is a C# Windows application designed to talk to the Tabmate and translate the button presses into key combinations that can be received by whatever application is in focus at the time (the Active Window). You configure in Tabmate Relay what sequences you want to be associated with which button press. The other program could be any program, not necessarily an art application.

For more information see:
See https://kenevans.net/opensource/TabmateRelay/Help/Overview.html

**Installation**

If you are installing from a download, just unzip the files into a directory somewhere convenient. Then run it from there. If you are installing from a build, copy these files and directories from the bin/Release directory to a convenient directory.

* Newtonsoft.Json.dll
* SharpLib.Hid.dll
* SharpLibWin32.dll
* System.Buffers.dll
* System.Memory.dll
* System.Numerics.Vectors.dll
* System.Resources.Extensions.dll
* System.Runtime.CompilerServices.Unsafe.dll
* Tabmate Relay.exe
* Tabmate Relay.exe.config
* Tabmate Relay.pdb
* Utils.dll
* Utils.pdb
* WindowsInput.dll
* Help
* InTheHand.Net.Bluetooth.dll
* LICENSE

To uninstall, just delete these files.

**More Information**

More information and FAQ are at https://kennethevans.github.io as well as more projects from the same author.

Licensed under the MIT license. (See: https://en.wikipedia.org/wiki/MIT_License)