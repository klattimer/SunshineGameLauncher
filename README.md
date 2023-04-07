# Sunshine Game Launcher

Sunshine game launcher is a simple VS project to wrap the launching of Steam and 
Epic Game Launcher titles. The intention is simply to execute the game without fuss,
hiding the desktop and exiting appropriately when the game exits.

## Usage 

Firstly you need to install [sunshine server](https://docs.lizardbyte.dev/projects/sunshine/en/latest/about/overview.html) on your game system. 

Now when you add a game, you can execute the binary for sunshine game launcher like so:

```
c:\games\sunshinegamelauncher.exe "com.epicgames.launcher://apps/b30b6d1b4dfd4dcc93b5490be5e094e5%3A22a7b503221442daa2fb16ad37b6ccbf%3AHeather?action=launch&silent=true" RDR2
```

The first argument is the link as created by the desktop shortcut generator in epic or steam. and the second is the executable name to monitor. Do not include the .exe at the end. 

There are various game level hacks necessary to stop blockages on streaming for instance:

 - Tombraider - add the ```-nolauncher``` command line option
 - Control - add the ```-dx12``` or ```-dx11``` command line option. 

 ## Adding command line options in steam 

  1. Click the gear icon for the game
  1. Select properties
  1. Edit the launch options
  
## Adding command line options in epic games launcher 

  1. Click your account icon
  1. Select settings 
  1. Scroll down to the game 
  1. Click the "Additional command line arguments checkbox"
  1. Edit the text in the unlabeled box
