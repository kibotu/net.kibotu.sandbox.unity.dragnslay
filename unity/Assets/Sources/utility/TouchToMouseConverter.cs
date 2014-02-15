using UnityEngine;

namespace Assets.Sources.utility
{
    /**
    Converts Mobile Touch to the default Mouse Click

    ## invoked these method in Mobile too
    OnMouseDown
    OnMouseDrag
    OnMouseUp 


    ## only called in touch screen
    OnPinchStart
    OnPinch(rate:float);
    OnPinchEnd


    @author koki ibukuro // https://gist.github.com/asus4/1674998
    */
    #region Namespaces

    

    #endregion
    
    [AddComponentMenu("Input/Touch To Mouse Converter")]
    public class TouchToMouseConverter : MonoBehaviour
    {
        /**
         State for convert mouse event
        */
        private enum TouchToMouseState
        {
            Down,
            Drag,
            Up,
            Pinching,
            PinchEnd
        }

        public bool isOn = true;

        private TouchToMouseState touchState = TouchToMouseState.Up;
        private RaycastHit hit;
        private Transform beforeTransform;

        //private float DRAGGING_DISTANSE = 100f;

        static private TouchToMouseConverter instance;
        static Camera cam;
        static Transform camTransform;

        static private int kRaycastLayers = 1 << 0;   // 1 << 8  Physics

        /// <summary>
        /// 	returns mouse or first touched point
        /// </summary>
        /// <returns></returns>
        static public Vector3 GetMousePosition()
        {
            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            }
            return Input.mousePosition;
        }


        /// <summary>
        /// 	returns mouse or touched point in WorldData
        /// </summary>
        static public Vector3 GetWorldMousePosition()
        {
            Vector3 mousePosition = GetMousePosition();
            Vector3 cameraPosition = camTransform.position;
            return cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cameraPosition.z));
        }

        static public TouchToMouseConverter GetInstance()
        {
            return instance;
        }


        /// <summary>
        /// 	set Using camera
        /// </summary>
        static public void SetCamera(Camera c)
        {
            cam = c;
            camTransform = c.transform;
        }

        void Awake()
        {
            if (instance)
            {
                Debug.LogError("ToughToMouseConverter class is Singleton. use GetInstance()");
                return;
            }
            instance = this;
        }

        void Start()
        {
            if (!cam)
            {
                SetCamera(Camera.main);
            }
        }

        void Update()
        {
            if (!isOn)
            {
                return;
            }
            int count = Input.touchCount;
            if (count == 1)
            {
                if (touchState == TouchToMouseState.Pinching)
                {
                    touchState = TouchToMouseState.PinchEnd;
                    toPinchEnd();
                }
                else if (touchState == TouchToMouseState.PinchEnd)
                {
                    // nothing to do
                }
                else
                {
                    singleTouch();
                }
            }
            else if (count >= 2)
            {
                multiTouch();
            }
            else
            {
                noTouch();
            }
            callGC();
        }

        private void singleTouch()
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = cam.ScreenPointToRay(touch.position);

            // if it has hitting object....
            //if ( !UIManager.instance.DidAnyPointerHitUI() && Physics.Raycast( ray, hit, Mathf.Infinity, kRaycastLayers) )  {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, kRaycastLayers))
            {
                // start touch
                if (touch.phase == TouchPhase.Began)
                {
                    toDown();
                }
                // touch move
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (beforeTransform == hit.transform)
                    {
                        toDrag();
                    }
                    else
                    {
                        toMouseExit();
                        toDown();
                    }
                }
                // end touch
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    toUp();
                }

                if (touchState == TouchToMouseState.Drag && hit.transform != beforeTransform)
                {
                    toMouseExit();
                }
            }
            else
            {
                // if dragging,,, send message to the old touched object
                if (touchState == TouchToMouseState.Drag)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        toOldDrag();
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        toMouseExit();
                    }
                }
            }
        }

        private float _touchesDistance = 0;
        private void multiTouch()
        {
            Touch a = Input.GetTouch(0);
            Touch b = Input.GetTouch(1);
            if ((a.phase == TouchPhase.Stationary || a.phase == TouchPhase.Moved)
             && (b.phase == TouchPhase.Stationary || b.phase == TouchPhase.Moved))
            {
                Ray ray = cam.ScreenPointToRay(a.position);

                // There are hitting objects..
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, kRaycastLayers))
                {
                    float d = Vector2.Distance(a.position, b.position);
                    if (touchState != TouchToMouseState.Pinching)
                    {
                        _touchesDistance = d;
                        toPinchStart();
                        touchState = TouchToMouseState.Pinching;
                    }
                    else
                    {
                        toPinch(d / _touchesDistance);
                    }
                }
                else
                {

                }
            }
        }

        private void noTouch()
        {
            if (touchState == TouchToMouseState.Pinching)
            {
                touchState = TouchToMouseState.PinchEnd;
            }
            else if (touchState == TouchToMouseState.PinchEnd)
            {
                touchState = TouchToMouseState.Up;
            }
            else if (beforeTransform)
            {
                toMouseExit();
            }
        }

        private void callGC()
        {
            if (Time.frameCount % 30 == 0)
            {
                System.GC.Collect();
            }
        }

        private void toDown()
        {
            hit.transform.SendMessage("OnMouseDown", null, SendMessageOptions.DontRequireReceiver);
            touchState = TouchToMouseState.Down;
            beforeTransform = hit.transform;
        }

        private void toDrag()
        {
            hit.transform.SendMessage("OnMouseDrag", null, SendMessageOptions.DontRequireReceiver);
            touchState = TouchToMouseState.Drag;
            beforeTransform = hit.transform;
        }

        private void toUp()
        {
            hit.transform.SendMessage("OnMouseUp", null, SendMessageOptions.DontRequireReceiver);
            touchState = TouchToMouseState.Up;
            beforeTransform = null;
        }

        private void toOldDrag()
        {
            beforeTransform.SendMessage("OnMouseDrag", null, SendMessageOptions.DontRequireReceiver);
        }

        private void toMouseExit()
        {
            if (beforeTransform) beforeTransform.SendMessage("OnMouseExit", null, SendMessageOptions.DontRequireReceiver);

            beforeTransform = null;
        }

        private void toPinchStart()
        {
            hit.transform.SendMessage("OnPinchStart", null, SendMessageOptions.DontRequireReceiver);
        }

        private void toPinch(float rate)
        {
            hit.transform.SendMessage("OnPinch", rate, SendMessageOptions.DontRequireReceiver);
        }

        private void toPinchEnd()
        {
            hit.transform.SendMessage("OnPinchEnd", null, SendMessageOptions.DontRequireReceiver);
        }
    }

}
