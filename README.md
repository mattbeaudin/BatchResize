# BatchResizer
A dead simple batch image resizer that's efficient and easy to use.

## Getting Started
If you're going to be doing development that doesn't involve adding more dependencies all you need to do is clone the repo and open it inside of Visual Studio.

If you plan on making changes that involve adding dependencies then you're going to need [WiX Toolset.](http://wixtoolset.org/) This is the tool I use to create the Windows installation package using XML.


## Compiling the Windows Installer

* (Optional) Set candle.exe and light.exe as Environment Variables inside of 'path'.
1. Build Solution as **Release.**
2. Open up the command prompt inside of the /Installer directory and run the following commands:

### With Environment Variables:
```
> candle BatchResizeInstaller.wxs
> light -ext WixUIExtension BatchResizeInstaller.wixobj
```

### Without Environment Variables:
```
> "C:/Program Files (x86)/WiX Toolset v3.11/bin/candle.exe" BatchResizeInstaller.wxs

> "C:/Program Files (x86)/WiX Toolset v3.11/bin/light.exe" -ext WixUIExtension BatchResizeInstaller.wixobj
```

### Note:
You only have to interact with the Windows installation package if you're adding new dependencies to the project.

## Installing
Download the latest installer from [the releases page.](https://github.com/mattbeaudin/BatchResize/releases)

## License
This project is licensed under the MIT License.
