/* to do list & ideas

IDEAS:
you can choose every unit and change to its FPS view to control it
fog of war
recycliong of wrecks
idea of: energy-attached units (so other complex buildings can created with combination of these units)
quit button: destroy user interface :D
game statistics at the end of a game
second selection mode (classic window dragging)
(buildings build itself up, but it can speed up with worker units)
buildings with worker units, which collect resources and build buildings
researches only to new units (but old units can manually be upgraded to new values)
units have energy, wich can create together a larger unit

CORE:
-lags on camera?
-selection: click through object? (make Ray from CameraControl public)
(>use ray to prevent short deselection gap when click on selected object)
-selection: double click > leaveSameObjectsSelected(...) [deactivated: methods don't work :( ]
-shortcut system: disable unity's ctrl+1 hotkey to check the shift+1 add-selections
-show current meetingpoint on minimap?
-production of units:	> replace unit if it gets set into another IO_Building or IO_Neutral
						> if several equal buildings are selected, its only possible to produce units!
-building: add HP within build process
-set meeting point/move: complete Action classes for use from the ActionMatrix (and not right click)
-attack method
-distraction when two objects collide (collisions,rigidbody,fixed joint)
 >graph: +only generate once at gamestart and remove nodes with each building/neutral process
		 +performance!
		 +calculation as background routine?


Documentation.cs:
-add key/button/shortcut functions

###COMMIT CLIPBOARD###
*/
