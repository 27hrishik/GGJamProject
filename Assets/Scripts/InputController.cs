using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public float moveForwardSpeed;
    public float rotationSpeed;
    public GameObject RedPrefab;
    public Rigidbody rigidbody;
    public Transform forwardTransform;
    public Transform handTransform;
    public Transform handTransformMain;
    public float healthMax;
    public Animator animator;
    float currentHealth;
    public Material light;
    public bool playerdead = false;
    Color color;

    public float sensitivityX = 10f;
    public float sensitivityY = 10f;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    GameObject temp;
    public int ifAccessKeyObtained;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = healthMax/1.5f;
        ifAccessKeyObtained = 0;
        color = new Color(0.2784f, 0.2823f, 1f);
        CalculateIntensity();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal").Equals(0f) && Input.GetAxis("Vertical").Equals(0f))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            rigidbody.angularVelocity = new Vector3(0f,0f,0f);
            float moveForward = Input.GetAxis("Vertical") * moveForwardSpeed * Time.deltaTime;
            float moveSide = Input.GetAxis("Horizontal") * moveForwardSpeed * Time.deltaTime;
            Vector3 forwardDirection = new Vector3(forwardTransform.forward.x, 0f, forwardTransform.forward.z).normalized;
            Vector3 sideDirection = new Vector3(forwardTransform.right.x, 0f, forwardTransform.right.z).normalized;
            rigidbody.MovePosition(transform.position + forwardDirection * moveForward + sideDirection * moveSide);
        }
        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("charge", true);
            HoldCharge();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("charge", false);
            FireCharge();
        }
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

    }

    void HoldCharge()
    {
        temp = Instantiate(RedPrefab, handTransform.position, forwardTransform.rotation);
        temp.transform.parent = forwardTransform;
        temp.GetComponent<MoveForward>().Speed = 0f;
       
    }

    void FireCharge()
    {
        temp.GetComponent<MoveForward>().Speed = 10f;
        temp.transform.rotation = forwardTransform.rotation;
        Destroy(temp, 10f);
        Damage(100f);
    }

    public void PlayerDeath()
    {
        playerdead = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Damage(float value)
    {
        currentHealth -= value;
        currentHealth = Mathf.Max(0f, currentHealth);
        CalculateIntensity();
    }

    public void AddIntensity(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Min(currentHealth, healthMax);
        CalculateIntensity();
    }

    public void CalculateIntensity()
    {
        //light.intensity = currentHealth / healthMax;
        float value = currentHealth / healthMax;
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V); 
        Color col = Color.HSVToRGB(H,S,value);
        light.SetColor("_EmissionColor", col);
        if (currentHealth.Equals(0f))
            PlayerDeath();
    }

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "RegenerationPoint" :
                AddIntensity(5f);
                break;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Collectibles":
                Destroy(other.gameObject);
                ifAccessKeyObtained++;
                break;
        }
    }


}
