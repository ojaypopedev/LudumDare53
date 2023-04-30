using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Droplet : MonoBehaviour
{

    Renderer rend => GetComponent<Renderer>();
    Material mat => rend.material;
    Rigidbody rb => rend.GetComponent<Rigidbody>();

    [SerializeField] ParticleSystem ps;

    public bool usePhysics = false;

    public float speedMultiplier = 0.25f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(usePhysics)
        mat.SetVector("_Velocity", rb.velocity * speedMultiplier);    
    }

    private void Start()
    {
        prevPosition = transform.position;
    }
    Vector3 prevPosition = Vector3.zero;

    private void Update()
    {
        if(!usePhysics)
        {
            if (prevPosition != transform.position)
            {
                mat.SetVector("_Velocity", Vector3.Lerp(mat.GetVector("_Velocity"), (transform.position - prevPosition) * speedMultiplier, 5 * Time.deltaTime));
                prevPosition = Vector3.Lerp(prevPosition, transform.position, Time.deltaTime*10);
            }
        }

    }

    public void Collide(Vector3 normal)
    {
        ps.transform.parent = null;
        ps.gameObject.SetActive(true);
        gameObject.SetActive(false);

        ps.transform.forward = normal;
        Destroy(gameObject, 3);
        Destroy(ps, 3);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Collide(collision.contacts[0].normal);
    }
}
