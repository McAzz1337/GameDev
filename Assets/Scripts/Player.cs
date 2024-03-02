using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveForce = 30.0f;
    [SerializeField] private float maxMoveVelocity = 10.0f;

    [SerializeField] private Weapon defaultWeapon;
    [SerializeField] private MolotovCocktailMock molotov;


    [Header("Debug Info")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private Vector2 moveInput;
    private PlayerControls controls;
    private Rigidbody rb;

    private Transform molotovTransform;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();

        controls = new PlayerControls();

        moveInput = Vector2.zero;

        Transform[] transforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in transform)
        {

            switch (t.gameObject.name)
            {

                case "MolotovTransform": molotovTransform = t; break;
            }
        }

    }

    void OnEnable()
    {

        controls.BattleControls.Move.performed += onBattleControlsPerformed;
        controls.BattleControls.Move.canceled += onBattleControlsCanceled;
        controls.BattleControls.Shoot.performed += onShootPerformed;
        controls.BattleControls.Throw.performed += onThrowPerformed;
        controls.Enable();
    }

    void OnDisable()
    {

        controls.BattleControls.Move.performed -= onBattleControlsPerformed;
        controls.BattleControls.Move.canceled -= onBattleControlsCanceled;
        controls.BattleControls.Shoot.performed -= onShootPerformed;
        controls.BattleControls.Throw.performed -= onThrowPerformed;
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

        if (weapon is MolotovCocktailMock) return;

        weapon?.shoot();
    }

    public void onThrowPerformed(InputAction.CallbackContext c)
    {

        if (!(weapon is MolotovCocktailMock)) return;

        weapon.transform.SetParent(null);
        weapon?.shoot();

        weapon = defaultWeapon;
    }


    void Start()
    {

        weapon = defaultWeapon;
    }

    void Update()
    {

        setLookRotation();
    }

    void FixedUpdate()
    {

        move();
    }

    private void move()
    {

        if (moveInput.x == 0.0f)
        {

            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }
        if (moveInput.y == 0.0f)
        {

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0.0f);

        }

        if (rb.velocity.magnitude > maxMoveVelocity)
        {

            rb.velocity = rb.velocity.normalized * maxMoveVelocity;
        }

        Vector3 moveVec = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized * moveForce;
        rb.AddForce(moveVec);
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

    public void pickupWeapon(Weapon weapon)
    {

        this.weapon = weapon;

        if (weapon is MolotovCocktailMock)
        {

            GameObject g = weapon.gameObject;

            g.transform.position = molotovTransform.position;
            g.transform.rotation = molotovTransform.rotation;
            g.transform.SetParent(molotovTransform);
        }
    }

    void OnCollisionEnter(Collision collider)
    {

        int layer = 1 << collider.gameObject.layer;
        if (layer == LayerMask.GetMask("Wall"))
        {



        }

    }

}
