using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Collider kolider;
    [SerializeField] Rigidbody rb;
    [SerializeField] float force;
    [SerializeField] LineRenderer aimLine;
    [SerializeField] Transform aimWorld;



    bool shootingMode;
    bool shoot;
    float forceFactor;
    int shootCount;
    Vector3 forceDirection;
    Ray ray;
    Plane plane;

    public bool ShootingMode { get => shootingMode; }
    public int ShootCount { get => shootCount; }

    public UnityEvent<int> onBallShooted = new UnityEvent<int>();

    private void Update()
    {

        if (shootingMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                aimLine.gameObject.SetActive(true);
                aimWorld.gameObject.SetActive(true);
                plane = new Plane(Vector3.up, this.transform.position);
            }
            
            else if (Input.GetMouseButton(0))
            {

                //  Membuat arah untuk seberapa jauh mouse di drag (Force direction)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.Raycast(ray, out var distance);
                forceDirection = this.transform.position - ray.GetPoint(distance);
                forceDirection.Normalize();


                //  Mengihitung seberapa jauh mouse di drag (Force Factor)
                var mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                var ballViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
                var pointerDirection = ballViewportPos - mouseViewportPos;  // Menjadikan direction
                pointerDirection.z = 0; // Posisi Z pada direction dihilangkan
                pointerDirection.z *= Camera.main.aspect;
                pointerDirection.z = Mathf.Clamp(pointerDirection.z, -0.5f, 0.5f);
                forceFactor = pointerDirection.magnitude * 2; // Dikali 2 agar nilai forceFactor == 1, nilai defalut adalah 0.5


                //  Visual Aim
                aimWorld.transform.position = this.transform.position;
                aimWorld.forward = forceDirection;
                aimWorld.localScale = new Vector3(1, 1, 0.5f + forceFactor);


                var ballScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                var mouseScreenPos = Input.mousePosition;
                ballScreenPos.z = 1;
                mouseScreenPos.z = 1;
                var positions = new Vector3[]{
                    Camera.main.ScreenToWorldPoint(ballScreenPos), 
                    Camera.main.ScreenToWorldPoint(mouseScreenPos)};
                aimLine.SetPositions(positions);
                aimLine.endColor = Color.Lerp(Color.yellow, Color.red, forceFactor);
            }

            else if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
                shootingMode = false;
                aimLine.gameObject.SetActive(false);
                aimWorld.gameObject.SetActive(false);
            }
        }

        /*
            Cara melakukan raycast dengan manual
        */
        // if (Input.GetMouseButtonDown(0))
        // {
        //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //     if (Physics.Raycast(ray, out var hitInfo, 100) && hitInfo.collider == kolider)
        //     {
        //         shoot = true;
        //     }
        // }
    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            shoot = false;
            AddForce(forceDirection * force * forceFactor, ForceMode.Impulse);
            shootCount += 1;
            onBallShooted.Invoke(shootCount);
        }

        if (rb.velocity.sqrMagnitude < 0.01f && rb.velocity.sqrMagnitude != 0)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
    }

    public bool IsMove()
    {
        return rb.velocity != Vector3.zero;
    }


    public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
    {
        rb.useGravity = true;
        rb.AddForce(force, forceMode);
    }


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (this.IsMove())
        {
            return;
        }

        shootingMode = true;
    }
}