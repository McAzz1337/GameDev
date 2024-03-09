using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerNetwork : NetworkBehaviour
{

    public static PlayerNetwork localPlayer;

    [Header("Movement")]
    [SerializeField] private float moveForce = 30.0f;
    [SerializeField] private float maxMoveVelocity = 10.0f;

    [Header("References")]
    [SerializeField] private Transform weaponTransform;

    [Header("Debug Info")]
    [SerializeField] private bool isHost;
    [SerializeField] private bool isOwner;
    [SerializeField] private bool isClient;
    [SerializeField] private WeaponNetwork weapon;


    private NetworkVariable<Vector2> moveInput =
        new NetworkVariable<Vector2>(Vector2.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    Rigidbody rb;

    PlayerControls controls;

    void Awake()
    {

        localPlayer = this;
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {


        controls.BattleControls.Move.performed += onMovePerformed;
        controls.BattleControls.Move.canceled += onMoveCanceled;
        controls.BattleControls.Shoot.performed += onShootPerformed;
        controls.BattleControls.Throw.performed += onThrowPerformed;
        controls.Enable();
    }

    void OnDisable()
    {

        controls.BattleControls.Move.performed -= onMovePerformed;
        controls.BattleControls.Move.canceled -= onMoveCanceled;
        controls.BattleControls.Shoot.performed -= onShootPerformed;
        controls.BattleControls.Throw.performed -= onThrowPerformed;
        controls.Disable();
    }

    public void onMovePerformed(InputAction.CallbackContext c)
    {

        if (!IsOwner) return;

        moveInput.Value = c.ReadValue<Vector2>();
    }

    public void onMoveCanceled(InputAction.CallbackContext c)
    {

        if (!IsOwner) return;

        moveInput.Value = c.ReadValue<Vector2>();
    }

    public void onShootPerformed(InputAction.CallbackContext c)
    {


        if (!IsOwner) return;

        if (weapon == null) return;

        weapon.shoot();

        if (weapon.isEmpty())
        {

            dropWeapon();
        }
        else if (weapon as MolotovCocktailMock != null)
        {

            weapon = null;
        }

    }

    public void onThrowPerformed(InputAction.CallbackContext c)
    {

        /*
        if (molotov == null) return;

        molotov.transform.SetParent(null);
        molotov.shoot();

        molotov = null;
        */
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        isHost = IsHost;
        isOwner = IsOwner;
        isClient = IsClient;


    }

    void Start()
    {


    }

    void Update()
    {

        if (!IsOwner) return;

        setLookRotation();
    }

    void FixedUpdate()
    {

        if (!IsOwner) return;

        moveServerRpc();
    }

    [ServerRpc]
    public void moveServerRpc()
    {

        move();
    }

    private void move()
    {



        if (moveInput.Value.x == 0.0f)
        {

            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
        }
        if (moveInput.Value.y == 0.0f)
        {

            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0.0f);

        }

        if (rb.velocity.magnitude > maxMoveVelocity)
        {

            rb.velocity = rb.velocity.normalized * maxMoveVelocity;
        }

        Vector3 moveVec = new Vector3(moveInput.Value.x, 0.0f, moveInput.Value.y).normalized * moveForce;
        rb.AddForce(moveVec);
    }


    [ServerRpc]
    private void setLookRotationServerRpc(Quaternion rot)
    {

        transform.rotation = rot;
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

            setLookRotationServerRpc(rot);
        }


    }

    public void pickupWeapon(WeaponNetwork weapon)
    {

        this.weapon = weapon;
        weapon.transform.position = weaponTransform.position;
        weapon.transform.rotation = weaponTransform.rotation;
        weapon.transform.SetParent(transform);
    }




    public void dropWeapon()
    {

        weapon.drop();
        weapon = null;
    }


    public bool canPickupWeapon()
    {

        return weapon == null;
    }

}
