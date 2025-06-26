using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody _rb;
    public Transform _planet;
    public float _moveSpeed = 2500f;
    private float tempSpeed;
    
    public float _radius;
    public float _rotationSpeed = 90f;
    public bool _isGrounded = true;
    public float _jumpForce = 6000f;
    public float _gravity = 400f;

    public float _knockAmount = 100f;

    void Start()
    {
         tempSpeed = _moveSpeed / 2;
    }

    void FixedUpdate()
    {
         Vector3 toCenter = (transform.position  - _planet.position).normalized * 2 ;
        Vector3 tangent = Vector3.Cross(transform.right, toCenter).normalized;
        
        
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, _rotationSpeed * Input.GetAxis("Horizontal"), 0) * Time.fixedDeltaTime);
        _rb.MoveRotation( Quaternion.LookRotation(tangent, toCenter) * deltaRotation );
        _rb.linearVelocity = tangent * _moveSpeed * Time.deltaTime;
        

         if(Vector3.Distance(transform.position, _planet.position) < _radius + 0.1f)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
        
        
        
        _rb.AddForce(toCenter * -_gravity );
        
    }

    void Update()
    {
        Vector3 toCenter = (transform.position  - _planet.position).normalized * 2 ;
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("HUI");
            if (_isGrounded)
            {
                _rb.AddForce(toCenter * _jumpForce);
            }
                
         }
    }

    public void DoKnockback(Vector3 pos)
    {
        StartCoroutine(Knockback(pos));
    }
    private IEnumerator Knockback(Vector3 pos)
    {
        float temp = _moveSpeed;
        _moveSpeed = 0;
        Debug.Log(pos + " " + transform.position);
        Vector3 toCenter = (transform.position  - _planet.position).normalized * 2 ;
        Vector3 dir = -transform.forward * _knockAmount + toCenter * _knockAmount;
        Debug.DrawRay(transform.position, dir, Color.green , 2, false);
        _rb.AddForce(dir, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        _moveSpeed = tempSpeed;
        yield return new WaitForSeconds(5f);
        _moveSpeed = temp;
    }



}