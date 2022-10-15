using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // used for event system components.

// a script for tracking the location of the mouse and having it interact with objects.
// this uses the main camera.
public class Mouse : MonoBehaviour
{
    // the camera the mouse is used. Default to the main camera.
    // public Camera camera;

    // the mouse key for mouse operations. The default is Keycode.Mouse0, which is the left mouse button.
    public KeyCode mouseKey = KeyCode.Mouse0;

    // the world position of the mouse.
    public Vector3 mouseWorldPosition;

    // if set to 'true', the UI is ignored for raycasting.
    // if set to 'false', the UI can block a raycast.
    [Tooltip("if true, the UI is ignored for raycast collisions. If false, UI elements can block a raycast.")]
    public bool ignoreUI = true;

    [Header("Interactions")]

    // the object the mouse is hovering over.
    public GameObject hoveredObject = null;

    // the object that has been clicked and held on.
    // when the mouse button is released, this is set to null. This variable gets set to null when the mouse button is released.
    public GameObject heldObject = null;

    // the last object that was clicked on. The next time someone clicks on something, this will be set to null.
    public GameObject lastClickedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // // sets to the main camera.
        // if (camera == null)
        //     camera = Camera.main;
    }


    // checks to see if the cursor is in the window.
    public static bool MouseInWindow()
    {
        return MouseInWindow(Camera.main);
    }

    // checks to see if the cursor is in the window.
    public static bool MouseInWindow(Camera cam)
    {
        // checks area
        bool inX, inY;

        // gets the viewport position
        Vector3 viewPos = cam.ScreenToViewportPoint(Input.mousePosition);

        // check horizontal and vertical.
        inX = (viewPos.x >= 0 && viewPos.x <= 1.0);
        inY = (viewPos.y >= 0 && viewPos.y <= 1.0);

        return (inX && inY);
    }

    // gets the mouse position in screen space.
    public static Vector3 GetMousePositionInScreenSpace()
    {
        return Input.mousePosition;
    }

    // gets the mouse position in viewport space.
    public static Vector3 GetMousePositionInViewport()
    {
        return Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    // gets the mouse position in world space using the main camera.
    public static Vector3 GetMousePositionInWorldSpace()
    {
        return GetMousePositionInWorldSpace(Camera.main);
    }

    // gets the mouse position in world space.
    public static Vector3 GetMousePositionInWorldSpace(Camera cam)
    {
        // TODO: check and see if focal length is the best way to go about this.
        if (cam.orthographic) // orthographic (2d camera) - uses near clip plane so that it's guaranteed to be positive.
            return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        else // perspective (3d camera)
            return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.focalLength));
    }

    // gets the mosut target position in world space using the main camera.
    public static Vector3 GetMouseTargetPositionInWorldSpace(GameObject refObject)
    {
        // return GetMouseTargetPositionInWorldSpace(Camera.main, refObject);
        return GetMouseTargetPositionInWorldSpace(Camera.main, refObject.transform.position);
    }

    // gets the mouse target position in world space.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Camera cam, GameObject refObject)
    {
        // Vector3 camWPos = GetMousePositionInWorldSpace(cam);
        // Vector3 target = camWPos - refObject.transform.position;
        // return target;

        return GetMouseTargetPositionInWorldSpace(cam, refObject.transform.position);
    }

    // gets the mouse target position in world space with a reference vector.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Vector3 refPos)
    {
        return GetMouseTargetPositionInWorldSpace(Camera.main, refPos);
    }

    // gets the mouse target position in world space with a reference vector.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Camera cam, Vector3 refPos)
    {
        Vector3 camWPos = GetMousePositionInWorldSpace(cam);
        Vector3 target = camWPos - refPos;
        return target;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target; // ray's target
        Ray ray; // ray object
        RaycastHit hitInfo; // info on hit.
        bool rayHit; // true if the ray hit.

        // if 'ignoreUI' is true, this is always false (the UI will never block the ray).
        // if 'ignoreUI' is false, then a check is done to see if a UI element is blocking the ray.
        bool rayBlocked = (ignoreUI) ? false : EventSystem.current.IsPointerOverGameObject();

        // gets the mouse position.
        mouseWorldPosition = GetMousePositionInWorldSpace();

        // if the ray is not blocked.
        if (!rayBlocked)
        {
            // checks if the camera is perspective or orthographic.
            if (Camera.main.orthographic) // orthographic
            {
                // tries to get a hit. Since it's orthographic, the ray goes straight forward.
                target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                // ray position is mouse position in world space.
                ray = new Ray(mouseWorldPosition, target.normalized);

                // the max distance is the far clip plane minus the near clip plane.
                float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                // cast the ray about as far as the camera can see.
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
            }
            else // perspective
            {
                // the target of the ray.
                target = GetMouseTargetPositionInWorldSpace(Camera.main.gameObject);
                
                // ray object. It offsets so that objects not in the camera's clipping plane will be ignored.
                ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane, 
                    target.normalized);

                // the max distance is the far clip plane minus the near clip plane.
                float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                // the max distance
                rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
            }


            // checks if the ray got a hit. If it did, save the object the mouse is hovering over.
            // also checks if object has been clicked on.
            if (rayHit)
            {
                hoveredObject = hitInfo.collider.gameObject;

                // left mouse button has been clicked, so save to held object as well.
                if (Input.GetKeyDown(mouseKey))
                {
                    heldObject = hitInfo.collider.gameObject;
                    lastClickedObject = heldObject;
                }
            }
            else
            {
                // if the camera is orthographic, attempt a 2D raycast as well.
                if (Camera.main.orthographic)
                {
                    // setting up the 2D raycast for the orthographic camera.
                    RaycastHit2D rayHit2D = Physics2D.Raycast(
                        new Vector2(mouseWorldPosition.x, mouseWorldPosition.y),
                        new Vector2(target.normalized.x, target.normalized.y),
                        Camera.main.farClipPlane - Camera.main.nearClipPlane
                        );

                    // if a collider was hit, then the rayhit was successful.
                    rayHit = rayHit2D.collider != null;

                    // checks rayHit value.
                    if (rayHit)
                    {
                        // the ray hit was successful.
                        rayHit = true;

                        // saves the hovered over object.
                        hoveredObject = rayHit2D.collider.gameObject;

                        // left mouse button has been clicked, so save to clicked object as well.
                        if (Input.GetKeyDown(mouseKey))
                        {
                            heldObject = rayHit2D.collider.gameObject;
                            lastClickedObject = heldObject;
                        }
                    }
                }

                // if ray hit was not successful.
                // this means the 3D raycast failed, and the 2D raycast (orthographic only).
                if (!rayHit)
                {
                    // no object beng hovered over.
                    hoveredObject = null;

                    // mouse hasb een clicked down again.
                    if (Input.GetKeyDown(mouseKey))
                        lastClickedObject = null;
                }
            }
        }

        // left mouse button released, so clear clicked object.
        if (Input.GetKeyUp(mouseKey))
            heldObject = null;

    }
}
