using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7.0f;

    [SerializeField] private GunMock gun;

    [Header("Debug Info")]
    [SerializeField] private Vector2 moveInput;
    private PlayerControls controls;

    void Awake()
    {

        controls = new PlayerControls();
    }

    void OnEnable()
    {

        controls.BattleControls.Move.performed += onBattleControlsPerformed;
        controls.BattleControls.Move.canceled += onBattleControlsPerformed;
        controls.BattleControls.Shoot.performed += onShootPerformed;
        controls.Enable();
    }

    void OnDisable()
    {

        controls.BattleControls.Move.performed -= onBattleControlsPerformed;
        controls.BattleControls.Move.canceled -= onBattleControlsCanceled;
        controls.BattleControls.Shoot.performed -= onShootPerformed;
        controls.Disable();
    }

    public void onBattleControlsPerformed(InputAction.CallbackContext c)
    {

        moveInput = c.ReadValue<Vector2>();
    }

    public void onBattleControlsCanceled(InputAction.CallbackContext c)
    {

        moveInput = c.ReadValue<Vector2>();
    }

    public void onShootPerformed(InputAction.CallbackContext c)
    {

        gun.shoot();
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

            float t = (transform.position.y - ray.origin.y) / ray.direction.y;

            Vector3 position = ray.origin + t * ray.direction;

            Quaternion rot = Quaternion.LookRotation(position - transform.position, Vector3.up);
            rot.x = 0.0f;
            rot.z = 0.0f;

            transform.rotation = rot;
        }
    }

}
