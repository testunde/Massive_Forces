/* to do list & ideas

IDEAS:
you can choose every unit and change to its FPS view to control it
routing: a*
fog of war
recycliong of wrecks
idea of: energy-attached units (so other complex buildings can created with combination of these units)
quit button: destroy user interface :D
game statistics at the end of a game
second selection mode (classic window dragging)
(buildings build itself up, but it can speed up with worker units)
buildings with worker units, which collect resources and build buildings

CORE:
-lags on camera?
-selection: click through object? (make Ray from CameraControl public)
(>use ray to prevent short deselection gap when click on selected object)
-selection: double click > leaveSameObjectsSelected(...) [deactivated: methods don't work :( ]
-show current meetingpoint on minimap?
-production of units:	> replace unit if it gets set into another IO_Building or IO_Neutral
						> if several equal buildings are selected, its only possible to produce units!
-building: add HP within build process
-set meeting point/move: complete Action classes for use from the ActionMatrix (and not right click)
-meeting point: several movement calls if shift is hold down to do move actions
-moveTo method for IO_Unit
-attack method
-shortcut system (e.g.: ctrl+1)
-research system (how to handle/set factors?)


Documentation.cs:
-add key/button/shortcut functions

###COMMIT CLIPBOARD###
*/
