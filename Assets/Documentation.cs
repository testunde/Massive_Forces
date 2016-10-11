/*
####
shortcut overview
####

.


####
begin of documentation (german)
####

Hinweis:
Alle mit @ markierten Variablen/Methoden sind public, können also von anderen Skripts genutzt werden!
Auch nur dann sind die Datentypen in {} angegeben. Für private Var/Met, siehe im jeweiligen Skript.
Ein # vor einem Wort bedeutet, dass die Variable/Methode hinter dem # gemeint ist.

##> Skripts: [Assets/scripts] (nach Dateinamen sortiert)
Alle hier aufgeführten Skripts werden im Namespace SCRIPTS abgelegt.

<--Building.cs---
Script wird in jedes Gebäude kopiert. Hier können Gebäudeanimationen und -interaktionen definiert werden.
Eine simple Aufbauanimation ist enthalten (Gebäude wächst aus Boden) - siehe FixedUpdate().
Rotationen und Translationen werden für alle Objecte, daher sowohl Vorschau als auch dem richtigen Gebäudeobjekt gesetzt,
d.h. das Transform des Parentobjekts wird gesetzt.

@Init(CameraControl cC, State s, int fraction) > void
Manuelle Initialisireung. Damit kann das Stateobject (siehe State.cs) und der aktuelle Kameracontroller übergeben werden. Die zugehörige Fraction wird hier zugeteilt.

@getParent() > GameObject
Gibt das Gruppenobjekt zurück, welches die Buildskripts und Gebäudeobjekte enthält.

@getName() > String
Gibt Gebäudenamen zurück, welches dem Objekt von zurückgegebenen #getParent() gleicht.

@getHP() > int
Gibt die aktuellen Lebenpunkte zurück.

@getMaxHP() > int
Gibt Obergrenze der Lebenpunkte zurück.

@changeHPBy(int HP) > void
Änder die aktuellen Lebenpunkte um den übergebenen Betrag. Kann d.h. auch negativ sein.
Ist die Änderung größer als #getMaxHP(), dann wird sie auf #getMaxHP() gesetzt.

@getFraction() > int | @setFraction(int fraction) > void
Gibt die aktuell zugewiesene Fraktion als ID zurück. | Setzt Fraktions-ID.

@createPreview(Material previewMat) > void
Erstellt das Vorschauobject zum Gebäudebau. Später kann das #previewMat auch direkt
mittels Resources.Load geladen werden.

@setCoords(Vector3 coords) > void
Verschiebt das Gebäude an die Position #coords.
ACHTUNG: Es wird nicht beachtet ob das Gebäude dem Terrain entsprechend auf der richtigen y-Koordinate positioniert wrid.

@rotateTo(Vector3 coords) > void
Rotiert das Gebäude mit der Front in Richtung von #coords NUR über die y-Achse.

@createBuilding() > void
Erstellt das Gebäudeobjekt mit den jeweiligen Rotationen und Translationen.
Im moment wird es um -2f in y-Richtung versetzt für die Aufbauanimation verschoben.
MouseReaction.cs wird als Komponente in das Gebäudeobjekt eingefügt, welches u.a. Selektionen verwaltet.

FixedUpdate()
Wenn Gebäudeobjekt gesetzt wurde, wird die down-Variable abgearbeitet.
(Gebäudeobjekt genau um selben Wert in y-Richtung verschoben)
--->


<--BuildMain.cs---
Hauptskritp zur Verwaltung aller Gebäude.
Entsprechende Tasten oder Befehle zur Auslösung von Gebäudeerstellung müssen hier definiert werden.
Wird ein Gebäude erstellt, so wird ein neues leeres GameObject erstellt und das jeweilige Gebäudeskript eingefügt,
welches als Schnitstelle dienen sollte.

@startBuild(int nr) > void
Startet den Bauprozess, bzw. man kann dann das Gebäude setzen. Wird nur ausgeführt, wenn kein anderer Prozess läuft.
Die übergebene #nr ist die ID des zu bauenden Gebäudes. Nur ID's größer 0 erlaubt!
Aktuelle Gebäude-ID's:
1- nur ein Würfel als Test

@getAliveBuildings() > List<Building>
Gibt die aktuellen (lebenden) Gebäude als seperate Liste des Typs Building zurück.
Seperat heißt: Es wird intern eine Kopie als Rückgabeliste erstellt, damit die intern geführte Gebäudeliste nicht zerstört werden kann. (da Listen Referenzobjekte sind)
--->


<--CameraMapCollision.cs---
NICHT IN VERWENDUNG!
--->


<--CameraControl.cs---
Allgemeiner Kameracontroller. Hier können u.a. sämtliche Tasteneingaben abgefangen werden.

mainCamera				erklärt sich von selbst ;)
camTr					Transform der mainCamera, für kürzere Addressierung
oldMapHigh,currentHigh	zur Zwischenspeicherung für die nächste Updateroutine
minHigh,maxHigh			Minimal-/Maximalhöhe der Kamera von der Mapoberfläche zum Scrollen (nur y-Achse wird betrachtet)
ground					aktueller Terrain
cube					der kleine süße Würfel, der später an die Stelle #pointer gesetzt wird, wenn #leftClick true ist
x,y,z					(auskommentiert) Koordinaten, genutzt für einen auskommentierten Code im Update-Teil
@mouseLook {MouseLook}	Objekt der eigenen MouseLook Klasse (siehe MouseLook.cs)
@pointer {Vector3}		aktuelle Position, auf den der Mauszeiger auf dem Terrain zeigt; wird bei jeder Updateroutine aktualisiert
@leftClick,shiftPress,ctrlPress {bool}	true, wenn linke Maustaste (up-Flanke); LeftShift (holdDown); LeftCtrl (holdDown)

@getCoordsAtXZ(Vector3 where) > {Vector3}
Gibt die y-Koordinate auf dem Terrain #ground bei x und z des #where-Vectors.
Übergebene y-Koordinate muss unter 0 liegen (bzw unter dem Terrain), da von diesem Punkt nach
oben "geschaut" wird bis das Terrain getroffen wird.
Gibt die y-Koordinate als Vector3 mit den alten x und z zurück.
Y-Koordinate wird um ein dekrementiert wenn das Terrain nicht getroffen wurde. (bleibt daher negativ)

Start()
Initialisierung.
Gesetzt wird: #mainCamera,#camTr,#gound
#cube wird erstellt und skaliert
--->


<--MouseLook.cs---
Eigener Klassen-/Objecttyp, der die Kameraschwenkung verwaltet.
Sensitivität NUR bzgl. der Kameraschwenkung kann hier nachträglich eingebracht werden!

@MouseLook() > Kontruktor

@LookRotation(Transform camera, float downMax, float upMax) > void
camera					zu kontrollierende Kamera
downMax,downMax			maximaler/minimaler Grad (in Gradmaß) der vertikalen Kameraschwenkung
Wenn Methode aufgerufen wird, wird die Kamera in Richtung der akuellen Mausbewegung
geschwenkt (nur Rotation, keine Translation).
--->


<--MouseReaction.cs---
Verwaltet (im Moment NUR) Selektionen, d.h. Materialveränderungen/-hervorhebungen.
Muss in das entsprechnde GameObject als Componente eingefügt werden.
Für Befehle, die beim Auswählen ausgeführt werden sollen: siehe Methode 'void OnMouseDown()'
Für Befehle, die beim Abwählen ausgeführt werden sollen: siehe Methode 'void Update()'

@Init(CameraControl cC, State s) > void
Manuelle Initialisireung. Damit kann das Stateobject (siehe State.cs) und der aktuelle Kameracontroller übergeben werden.
--->


<--State.cs---
Für die Statusverwaltung in Form von Integer. Damit ein Status übergeben werden kann und
dieser auch von anderen Klassen darauf in Echtzeit abgerufen werden kann,
dient diese Klasse als Refernzobjekt auf dessen Status.

@State() > Kontruktor

@Get() > int
Gibt den aktuellen Status zurück.

@Set(int s) > void
Setzt den Status auf #s.

@Equal(int s) > bool
Gibt true zurück, wenn der aktuelle Status #s entspricht.
--->

*/
