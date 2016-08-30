# UnityUIConstructor
A wrapper/helper for the Unity UI package to aid in quickly prototyping and making Unity UIs

Please note that this is still a work in progress.

It is based on the following concepts:

Coordinator
	An abstract monobehaviour class that is added to an empty game object inside a canvas
	Controls all settings
	Becomes the root of the UI tree
	Has all relevant functions and a reference is distributed among all 

Slides
	What might be considered a page or a screen (like a web page or a screen in an installation program or a slide in a powerpoint presentation).
	Contains panels
	Only one slide visible at a time
	Not necessarily full screen.
	Can move between them using transitions - a single method call. Transition are defined on start up to ensure consistency
	One starting slide, starting visibility selected in editor. Can be triggered at any point using the base coordinator class.

Panels
	Contains (sub)panels and controls
	Can be nested as much as required
	Integral for doing complex layouts
	Can be things like buttons or menu bars

Controls
	The end of the component tree, no sub components
	Can be things like text or sprites
