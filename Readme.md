<p align="center">
      <img src="https://i.ibb.co/9wQjwjP/Logo.png" alt="Logo" width="726" border="0">
</p>
<p align="center">
      <img src="https://img.shields.io/badge/Engine-Unity%202021.3.3f1-green">
      <img src="https://img.shields.io/badge/Game%20version-0.3-blue">
</p>

## About

Simple Clicker Game is incremental game that focused on production and consumption cycle.

## Technical issues

<b>Game entry point</b> is placed on scene as [Bootstrapper](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/Bootstrapper.cs"). It creates new [GameStateMachine](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/GameStateMachine.cs) and this load all needed services and load saved data if it exists.
States in that state machine divided by their functional. There are 3 in total: 
1. [InitServices](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/States/InitServicesState.cs) state - loads all services that needed game to work. 
2. [Init](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/States/InitState.cs) state - loads saved data or creating new and generating presenters for game elements.
3. [GameLoop](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/States/GameLoopState.cs) state - linking popup show invokes to generator production ended events.

<b>Game elements</b>:
- [Resource](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/Models/Resource.cs) - value that player increasing and spend. Has image and description.
- [Generator](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/Models/Generator.cs) - thing that produces resources. Has image, description, productivity and level. Can be upgraded by spending resources.
- [Manager](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/Models/Manager.cs) - thing that automatize generators. Has image, description, linked generator and resource cost. Allows generators to produce resources while player offline.

<b>Services</b>: 
- [LoaderService](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/Services/LoaderService.cs) - loads game assets by their type.
- [PopupService](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/Services/PopupService.cs) - shows popups in needed position.
- [BuyService](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/Services/BuyService.cs) - controls buy amount.
- [InformationService](https://github.com/fzfking/SimpleClickerGame/blob/main/Assets/Sources/GameLoop/Services/InformationService.cs) - shows information about item when player click on [i] button.


## Features

You can change and adjust this game as you want. 
All static data contains in Assets/Data/Resources/StaticData/StaticDataContainer.asset
<p align="center">
  <img src="https://i.ibb.co/wgqJ0LK/image.png" alt="image" border="0">
</p>
You can create new game resources and settings by context menu in assets field:<p align="center"><img src="https://i.ibb.co/jHYHXXB/Create-Menu.png" alt="Create-Menu" border="0"></p>

## Gameplay example
<p align="center">
  <img src="https://i.ibb.co/x2mhxfx/movie-006-1.gif" alt="Gameplay video">
</p>

## Distribution

- [itch.io](https://fzfstudios.itch.io/simple-clicker-game)
