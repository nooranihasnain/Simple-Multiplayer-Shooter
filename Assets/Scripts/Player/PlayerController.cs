using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    private PlayerMotor motor;

    // Use this for initialization
    void Start () {
        motor = GetComponent<PlayerMotor>();
    }
	
	// Update is called once per frame
	void Update () {

        if(PauseMenuScript.IsOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Get horizontal and vertical axis
        Vector3 MovHoz = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 MovVer = transform.forward * Input.GetAxisRaw("Vertical");

        Vector3 Velocity = (MovHoz + MovVer).normalized * speed;
        motor.Move(Velocity);

        //Rotation
        Vector3 Rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0f) * 5f;
        motor.Rotate(Rotation);

        //CameraRotation
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float CamRotation = _xRot * 5f;
        motor.RotateCamera(CamRotation);
    }
}
