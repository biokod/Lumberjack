using UnityEngine;
using System.Collections;

public class EnemyHelper : MonoBehaviour {

    public float speed;                                 // Текущая скорость паука.
    public float walkSpeed = 3.0f;                      // Скорость ходьбы.

    EnemyState State = EnemyState.Walk;

    Animator animator;
    public ParticleSystem blood;

    void Start()
    {
        animator = GetComponent<Animator>();
        speed = walkSpeed;

        InvokeRepeating("ChangeDirection", 1.0f, Random.Range(0.5f, 1.2f));
        InvokeRepeating("Jump", 1.0f, Random.Range(2.0f, 5.0f));
    }

    void Update()
    {
        Move();
        Animate();
    }

    void Move()
    {
        if (State == EnemyState.Walk)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
        }
    }

    /// Случайное изменение направления паука путем поворота на random'ный угол.
    void ChangeDirection()
    {
        transform.Rotate(transform.up, Random.Range(-45, 45));
    }

    void Jump()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.value * 1, 5, Random.value * 1), ForceMode.Impulse);
    }

    void Hit()
    {
        if (State != EnemyState.Dead)
        {
            State = EnemyState.Dead;
            speed = 0;

            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;

            //  С вероятностью 8/10 с паука будет drop'аться драгоценный камень.
            if (Random.Range(0, 10) > 2)
                Instantiate(Resources.Load<GameObject>("Diamond"), transform.position + transform.up, Quaternion.identity);

            blood.gameObject.SetActive(true);       // Активируем ParticleSystem, который отвечает за выливание крови.
            Destroy(gameObject, 3.0f);              // Через 3 секунды GameObject паука будет уничтожен.
        }
    }

    void Animate()
    {
        animator.SetInteger("State", (int)State);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bounds")       // Если у объекта, с которым столкнулись тег "Bounds", значит это границы игрового поля.
            transform.Rotate(transform.up, 180);
    }
}
