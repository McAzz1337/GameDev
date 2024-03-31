using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{

    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private float maxMoveVelocity = 10.0f;
    [SerializeField] private float moveForce = 30.0f;


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
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

    [ServerRpc]
    private void setLookRotationServerRpc(Quaternion rot)
    {

        transform.rotation = rot;
    }


    [ServerRpc]
    public void moveServerRpc()
    {

        move();
    }

    private void move()
    {


        Vector2 moveInput = playerInput.getMoveInput();
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


}