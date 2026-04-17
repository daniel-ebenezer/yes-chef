# yes-chef
Cooking game from Albequerque

Unity version 6000.3.8f1 (LTS) was used for this project.

# Architecture
All the features from the document have been implemented. Nevermind the UI, its quite simple.

I focused mostly on building a scalable project with loose coupling.

The architecture is simple,

IInteractable interface with a "BaseStation" abstract class for all interactables including the station, table, stove, fridge, trash.

They all have common functions that they share, so I did it this way.

Communication between scripts is done by using Scriptable objects as events, which is designer-friendly.

Config for the players , ingredient stats and types are done through scriptable objects for easier tweaking and testing.

Awaitables are used instead of coroutines for tasks like chopping, cooking.

# Could have done better

One slightly annoying mistake in the game you might encounter is sometimes the raycast misses the interactable because of the angle of the player, tested this very late. I felt i could have done it with trigger enter instead.

Object pooling for the ingredients.

# Bug

While testing in the morning, i figured i forgot to turn off the player movement in the start screen. So right now, you will be able to move the player around using WASD in the main menu. I've turned it off properly when in pause state and Fridge UI menu state. In case i dont fix it when you see the project, dont mind it please.

# Links 

itch.io (Playable in browser) - 

https://danielebenezer.itch.io/yes-chef

Password: 123

Youtube video of gameplay -

https://youtu.be/KYk6qYFk2oc

Windows build - 

Included in project/repo



