    var targetItem : GameObject;
    var GUICamera : Camera;
     
     
    /********Rotation Variables*********/
    var rotationRate : float = 1.0;
    private var wasRotating;
     
    /************Scrolling inertia variables************/
    private var scrollPosition : Vector2 = Vector2.zero;
    private var scrollVelocity : float = 0;
    private var timeTouchPhaseEnded: float;
    private var inertiaDuration : float = 0.5f;
     
    private var itemInertiaDuration : float = 1.0f;
    private var itemTimeTouchPhaseEnded: float;
    private var rotateVelocityX : float = 0;
    private var rotateVelocityY : float = 0;
     
     
    var hit: RaycastHit;
     
    private var layerMask = (1 << 8) | (1 << 2);
    //private var layerMask = (1 << 0);
     
     
    function Start()
    {
    layerMask =~ layerMask;
    }

    function FixedUpdate() {

        if (Input.touchCount > 0) { // If there are touches...
            var theTouch :
            Touch = Input.GetTouch(0); // Cache Touch (0)

            var ray = Camera.main.ScreenPointToRay(theTouch.position);


            if (Physics.Raycast(ray, hit, 50, layerMask)) {

                if (Input.touchCount == 1) {

                    if (theTouch.phase == TouchPhase.Began) {
                        wasRotating = false;
                    }

                    if (theTouch.phase == TouchPhase.Moved) {

                        targetItem.transform.Rotate(0, theTouch.deltaPosition.x * rotationRate, 0, Space.World);
                        wasRotating = true;
                    }

                }
            }
        }
    }