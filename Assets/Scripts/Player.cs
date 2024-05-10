using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

#region Variables Publicas

    public bool invertCamera = false;
    public float rotCameraZ = 0f;

    //Movimiento
    [Header("Parametros Movimiento Jugador")]
    public float maxSpeed = 4f;
    public float accSpeed = 0.1f;

    //Rotacion del personage
    [Space(5)]
    [Header("Parametros rotacion de la camara")]
    public float maxRotationSpeed = 20f;
    public float RotationSpeedMouse = 20f;
    public float accRotation = 0.1f;
    public float maxRotation = 60f;
    public float minRotation = -60f;

    

    [Space(5)]
    [Header("Otros")]
    public float gravity = -0.1f;

    [Space(5)]
    [Header("Referencias")]
    //Referencias
    public Camera camara;
#endregion

#region Variables Privadasq 
    private CharacterController _controller; //Referencia al character controller.
    private float speed = 0f; //Velocidad actual jugador.      
    private float rotationSpeedV = 0f; //Velocidad rotacion camara en el eje vertical.
    private float rotationSpeedH = 0f; //Velocidad rotacion camara en el eje horizontal.
    float h_rot = 0f; //Rotacion actual de la camara en horizontal.
    float v_rot = 0f; //Rotacion actual de la camara en vertical.
    private Vector3 mov; //Vector movimiento y direccion. 
    private float movAxiX = 0f; // input X  axis de movimiento
    private float movAxiY = 0f; // input Y  axis de movimiento
    private float joyRotateAxiX = 0f; //Input X axis  de rotacion solo del mando
    private float joyRotateAxiY = 0f; //Input Y axis  de rotacion solo del mando
    private float mouseRotateAxiX = 0f; // Input axis x raton.
    private float mouseRotateAxiY = 0f; //Input axis y raton
    bool mando = false; //Indica al juego si se esta usando un mando o no (actualmente el canvio solo se ve reflejado si el jugador rota la camara).
    
    enum Player_State {idle,walk,pause};
    Player_State currentState = Player_State.idle;
#endregion

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        camara = Camera.main;

        if(invertCamera) 
        {
            rotCameraZ = 180f;
        }

    }

    // Update is called once per frame
    void Update()
    {             
        switch(currentState)
        {
            case Player_State.idle:
                MovPlayer();
                Gravity();               
                break;
            case Player_State.walk:
                MovPlayer();
                Gravity();              
                 break;
            case Player_State.pause:
                break;
        }
        
        if(currentState != Player_State.pause) 
        {
            //Aplicacion del movimiento
            mov.x = mov.x * speed * Time.deltaTime; 
            mov.z = mov.z * speed * Time.deltaTime;
            _controller.Move(mov);  
        }

    }

    //Metodos privados
    void changeState(Player_State state)
    {
        currentState = state;
    }

    void PlayerRotateRaton()
    {
        //Teclado y raton
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            rotationSpeedH = mouseRotateAxiX * RotationSpeedMouse;
            rotationSpeedV = mouseRotateAxiY * RotationSpeedMouse;
        }
        else
        {
            rotationSpeedV = rotationSpeedH = 0;                
        }
       
        h_rot += rotationSpeedH * Time.deltaTime;
        v_rot -= rotationSpeedV * Time.deltaTime;       
        v_rot = Mathf.Clamp(v_rot, minRotation, maxRotation);
        camara.transform.localEulerAngles = new Vector3(v_rot, 0f, rotCameraZ);
        transform.localEulerAngles = new Vector3(0f, h_rot, 0f);
    }
    void PlayerRotate()
    {
        //Mando
        if (joyRotateAxiX != 0f)
        {
            rotationSpeedH += accRotation * Time.deltaTime;
        }
        else rotationSpeedH = 0f;

        if (joyRotateAxiY != 0f)
        {
            rotationSpeedV += accRotation * Time.deltaTime;
        }
        else rotationSpeedV = 0f;      
        
        rotationSpeedV = Mathf.Clamp(rotationSpeedV, -maxRotationSpeed * Mathf.Abs(joyRotateAxiY), maxRotationSpeed * Mathf.Abs(joyRotateAxiY));
        rotationSpeedH = Mathf.Clamp(rotationSpeedH, -maxRotationSpeed * Mathf.Abs(joyRotateAxiX), maxRotationSpeed * Mathf.Abs(joyRotateAxiX));
       
        h_rot -= joyRotateAxiX * rotationSpeedH * Time.deltaTime;
        v_rot -= joyRotateAxiY * rotationSpeedV * Time.deltaTime;
        v_rot = Mathf.Clamp(v_rot, minRotation, maxRotation);
        camara.transform.localEulerAngles = new Vector3(v_rot,0f,rotCameraZ);
        transform.localEulerAngles = new Vector3(0f, h_rot, 0f); 
    }

    void Gravity()
    {
        if (_controller.isGrounded) mov.y = 0f; else mov.y = gravity * Time.deltaTime;
      
    }
    void MovPlayer()
    {
        if (movAxiX != 0 || movAxiY != 0)  //Detecta si el jugador se mueve.
        {
            speed += accSpeed * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0f, maxSpeed);
            changeState(Player_State.walk);
        }
        else
        {
            changeState(Player_State.idle);
            speed = 0;
        }
       
        //Actualizamos la direccion del jugador segun los AXis
        mov.x = movAxiX;
        mov.z = movAxiY;
        mov = mov.normalized;

        mov = transform.TransformDirection(mov); //Canvia el vector direccion global a direccion local.

        //Logica para rotar la camara
        if (mando)
            PlayerRotate();
        else
            PlayerRotateRaton();       
    }
    //Metodos Publicos

    public void MovAxiX(InputAction.CallbackContext context)
    {
        movAxiX = context.ReadValue<float>();
       
    }

    public void MovAxiY(InputAction.CallbackContext context)
    {
        movAxiY = context.ReadValue<float>();    
        
    }

    public void JoyRotateAxiX(InputAction.CallbackContext context)
    {
        joyRotateAxiX = context.ReadValue<float>();
        mando = true;
    }

    public void JoyRotateAxiY(InputAction.CallbackContext context)
    {
        joyRotateAxiY = context.ReadValue<float>();
        mando = true;
    }

    public void MouseRotateAxiX(InputAction.CallbackContext context)
    {
        mouseRotateAxiX = context.ReadValue<float>();
        mando = false;
    }

    public void MouseRotateAxiY(InputAction.CallbackContext context)
    {
        mouseRotateAxiY = context.ReadValue<float>();
        mando = false;
    }
}
