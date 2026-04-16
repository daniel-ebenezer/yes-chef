# yes-chef
Cooking game from Albequerque

# Architecture
All the features from the document have been implemented. Nevermind the UI, its quite simple.

I focused mostly on building a scalable project with loose coupling.

The architecture is simple,

IInteractable interface with a "BaseStation" abstract class for all interactables including the station, table, stove, fridge, trash.

They all have common functions that they share, so I did it this way.

Communication between scripts is done by using Scriptable objects as events, which is designer-friendly.

# Could have done better

One slightly annoying mistake in the game you might encounter is sometimes the raycast misses the interactable because of the angle of the player, tested this very late. I felt i could have done it with trigger enter instead.

# Links 

itch.io (Playable in browser) - 

https://danielebenezer.itch.io/yes-chef

Password: 123

Youtube video of gameplay -

https://youtu.be/KYk6qYFk2oc



