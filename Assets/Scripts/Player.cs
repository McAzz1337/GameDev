using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 7.0f;

    [Header("Debug Info")]
    [SerializeField] private Vector2 moveInput;
    private PlayerControls controls;
    private Transform eyes;

    void Awake()
    {

        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in transforms)
        {

            if (t.gameObject.name == "Glasses")
            {

                eyes = t;
                break;
            }
        }

        controls = new PlayerControls();

    }

    void OnEnable()
    {

        controls.Walk.Move.performed += onWalkPerformed;
        controls.Walk.Move.canceled += onWalkPerformed;

        controls.Enable();
    }

    void OnDisable()
    {

        controls.Walk.Move.performed -= onWalkPerformed;
        controls.Walk.Move.canceled -= onWalkCanceled;
        controls.Disable();
    }

    public void onWalkPerformed(InputAction.CallbackContext c)
    {

        moveInput = c.ReadValue<Vector2>();
    }

    public void onWalkCanceled(InputAction.CallbackContext c)
    {

        moveInput = c.ReadValue<Vector2>();
    }


    void Start()
    {


    }

    void Update()
    {

        move();
        setLookRotation();
    }


    private void move()
    {

        transform.position += new Vector3(moveInput.x, 0.0f, moveInput.y) * moveSpeed * Time.deltaTime;
    }

    private void setLookRotation()
    {

        Ray ray = Camera.main.ScreenPointToRay(
              new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {

            float t = toEyeLevel(ray);

            Vector3 position = ray.origin + t * ray.direction;

            Quaternion rot = Quaternion.LookRotation(position - transform.position, Vector3.up);
            rot.x = 0.0f;
            rot.z = 0.0f;

            transform.rotation = rot;
        }
    }

    private float toEyeLevel(Ray ray)
    {
        return (eyes.position.y - ray.origin.y) / ray.direction.y;
    }
}
