var Speed=2;
//Add this script to a camera to make it move forward
//Add MouseLook.cs to the camera to turn the Camera
function Update () {
transform.Translate(Vector3.forward * Time.deltaTime * Speed);
}