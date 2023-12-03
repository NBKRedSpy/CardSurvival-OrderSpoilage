
![Food Spoil Order Example](SpoilOrder_small.png)

# Card Survival - Order Spoilage

Orders the food in coolers to be from soonest to spoil to latest.  This is based on the time remaining until spoil, and not the percentage.

# Note

Currently the sort only occurs when a Cooler is opened.

# Compatibility
Safe to add and remove from existing saves.

# Installation 
This section describes how to manually install the mod.

If using the Vortex mod manager from NexusMods, these steps are not needed.  

## Overview
This mod requires the BepInEx mod loader.

## BepInEx Setup
If BepInEx has already been installed, skip this section.

Download BepInEx from https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip

* Extract the contents of the BepInEx zip file into the game's directory:
```<Steam Directory>\steamapps\common\Card Survival Tropical Island```

    __Important__:  The .zip file *must* be extracted to the root folder of the game.  If BepInEx was extracted correctly, the following directory will exist: ```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx```.  This is a common install issue.

* Run the game.  Once the main menu is shown, exit the game.
    
* In the BepInEx folder, there will now be a "plugins" directory.

## Mod Setup
* Download the CardSurvival-OrderSpoilage.  
    * If on Nexumods.com, download from the Files tab.
    * Otherwise, download from https://github.com/NBKRedSpy/CardSurvival-OrderSpoilage/releases/

* Extract the contents of the zip file into the ```BepInEx/plugins``` folder.

* Run the Game.  The mod will now be enabled.

# Uninstalling

## Uninstall
This resets the game to an unmodded state.

Delete the BepInEx folder from the game's directory
```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx```

## Uninstalling This Mod Only

This method removes this mod, but keeps the BepInEx mod loader and any other mods.

Delete the ```AutoLoad.dll``` from the ```<Steam Directory>\steamapps\common\Card Survival Tropical Island\BepInEx\plugins``` directory.

# Change Log 
## 1.0.0  
* Initial Release

# Technical Note

This section is for modders and not relevant to users.

There is a sort button on the Inspection Dialog, but I was not able to get the slots to re-order.  The game would get stuck in a loop.
This is most likely to me not understanding how to force the game to cleanly re-order the inventory items to slot mappings.

Also, there is an "already has an assigned card!" message when removing cards from the middle of the inventory.  This is a similar issue that occurs with my Load Travois mod.

I haven't see any side effects.  It appears that the game's slot code self corrects this issue.  

If any modders have any knowledge in this area, I would appreciate any suggestions.


