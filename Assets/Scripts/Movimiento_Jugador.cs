using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento_Jugador : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    [Header("Animacion")]
    private Animator animator;
    private void Start()
    {
        rb2D=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator  >();
    }
    private void Update()
    {
     movHorizontal = Input.GetAxisRaw("Horizontal")*velocidadDeMovimiento;
     animator.SetFloat("Horizontal", Mathf.Abs(movHorizontal));
     if (Input.GetButtonDown("Jump"))
     {
        salto = true;
     }
    }
    private void FixedUpdate() 
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo",enSuelo);
        Mover(movHorizontal*Time.fixedDeltaTime,salto);
        salto = false;
    }
    private void Mover(float mover, bool salto )
    {
            Vector3 velocidadObjeto = new Vector2(mover,rb2D.velocity.y);
            rb2D.velocity=Vector3.SmoothDamp(rb2D.velocity,velocidadObjeto,ref velocidad, suavizadoDeMovimiento);

            if(mover > 0 && !mirandoDerecha)
            {
                //girar
                Girar();
            }
            else if(mover < 0 && mirandoDerecha)
            {
                //girar
                Girar();
            }
             if (enSuelo && salto)
            {
                enSuelo = false;
                rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
            }

    }
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale=escala;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }

}   