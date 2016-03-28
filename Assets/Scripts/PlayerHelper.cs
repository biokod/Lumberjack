using UnityEngine;
using System.Collections;

public class PlayerHelper : MonoBehaviour
{
    public int score = 0;

    public float speed = 0;
    public float walkSpeed = 3.0f;

    public float rotationValue = 105; // скорость поворота игрока.
    private bool standRotating = false;
    public float attackDuration = 0.75f;

    CharState State = CharState.Idle;
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        ChangeState();
        Move();
        Animate();
    }

    void Move()
    {
        standRotating = false;      // Сбрасываем флаг поворота на месте.
        if (Input.GetButton("Horizontal"))
        {   
            transform.Rotate(transform.up, rotationValue * Input.GetAxis("Horizontal") * Time.deltaTime);
            // Если игрок бездействует (Idle), то активирутся флаг поворота на месте. (true)
            standRotating = (State == CharState.Idle);
        }
        if (State == CharState.Walk)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
    }

    void ChangeState()
    {
        if (State == CharState.Attack) return;
        if (Input.GetMouseButtonDown(0))
        {
            State = CharState.Attack;
            speed = 0;
            Invoke("ToIdle", attackDuration);
            Invoke("Attack", attackDuration / 3);
        }

        if (State == CharState.Attack) return;
        if (Input.GetButton("Vertical"))
        {   
            State = CharState.Walk;
            speed = walkSpeed * Input.GetAxis("Vertical");
        }
        if (Input.GetButtonUp("Vertical"))
        {
            State = CharState.Idle;
            speed = 0;
        }
    }


    void Animate()
    {
        // Записываем в параметр State компонента Animator соответствующее значение состояния игрока.
        // В том случае, если игрок вращается на месте, включаем анимацию Walk.
        animator.SetInteger("State", (standRotating) ? (int) CharState.Walk : (int) State);
    }

    void Attack()
    {
        // Метод OverlapSphere возвращает массив колайдеров, которые касаются виртуальной сферы указаных параметров.
        Collider[] targets = Physics.OverlapSphere(transform.position + transform.forward / 2.0f, 0.4f);

        // Среди всех полученных колайдеров ищем колайдер паука.
        foreach (Collider target in targets)
            if (target.gameObject.GetComponent<EnemyHelper>())
            {
                target.gameObject.GetComponent<EnemyHelper>().Invoke("Hit", 0);
            }
    }

    void ToIdle()
    {
        State = CharState.Idle;
    }
}
