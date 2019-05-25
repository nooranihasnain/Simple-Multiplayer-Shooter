using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 Velocity = Vector3.zero;
    private Vector3 Rotation = Vector3.zero;
    private float CamRotationX = 0f;
    private float CurrentCamRotationX = 0f;
    private Rigidbody rb;
    [SerializeField]
    private Camera playerCam;
    
	void Start ()
	{
        rb = GetComponent<Rigidbody>();
	}
    //Physics movement
	void FixedUpdate ()
	{
        PerformMovement();
        PerformRotation();
    }

    //Gets velocity vector from PlayerController
    public void Move(Vector3 _velocity)
    {
        Velocity = _velocity;
    }

    //Gets Rotation vector from PlayerController
    public void Rotate(Vector3 _rotation)
    {
        Rotation = _rotation;
    }

    //Gets Camera Rotation vector from PlayerController
    public void RotateCamera(float _camRotation)
    {
        CamRotationX = _camRotation;
    }

    //Performs rotation using that vector
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Rotation));
        if(playerCam != null)
        {
            CurrentCamRotationX -= CamRotationX;
            CurrentCamRotationX = Mathf.Clamp(CurrentCamRotationX, -85f, 85f);
            playerCam.transform.localEulerAngles = new Vector3(CurrentCamRotationX, 0, 0);
        }
    }

    //Performs movement using that vector
    void PerformMovement()
    {
        if(Velocity!= Vector3.zero)
        {
            rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
        }
    }
}
