/// <summary>
/// Antrasis zaidejo judejimo skriptas(skirtas scenoje kirminas)
/// Pradzioje skyresi tuo kad neturejo stumdymo mechanikos bet ji dabar ir cia yra
/// Skirtumas nuo "PlayerMovement" yra knygos paemimo fiksavimas pagal tags daugiau viskas identiska
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWormMove : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float moveSpeed = 20;
    Vector2 direction;

    BoxCollider2D boxCollider;
    [SerializeField] LayerMask blockingLayer;

    [SerializeField] GameObject greenBookUI;
    public bool greenBook;
    public bool redBook;


    // Start is called before the first frame update
    //Pradzios darbai pasiemami objekto prie kurio prikabintas komponentai kad butu veliau lengviau
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    // Tinkamas klausyti paspaudimu ar kitu pasikeitimu nesusijusiu su fizika
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
    }

    //Iskvieciamas kai objektas i kuri atsitrenke turi Coliider2D atributa kuriame nustatyta "isTrigger = true"
    //Naudojamas patikrinti ar i ka atsitrenke yra knyga jei taip issaugome kad turime knyga ir paslepiame knyga i kuria atsitrenkeme
    void OnTriggerEnter2D(Collider2D other)
    {
        //tikriname ar atsitrenkeme i zalia knyga per tagus
         if(other.gameObject.tag == "greenBook")
        {
            greenBook = true; //pazymime kad turime zalia knyga kode
            other.gameObject.SetActive(false); //paslepiame knyga i kuria atsitrenkeme
            greenBookUI.SetActive(true); // parodome desineje ikona kad turime knyga
        }

        if(other.gameObject.tag == "redBook")
        {
            redBook = true;
            other.gameObject.SetActive(false);
        }

    }

    //Iskvieciamas kai objektas i kuri atsitrenke turi Coliider2D atributa (paprastai pasakius iskvieciamas kai atsirenkeme i ka nors)
    //Sioje situacijoje atliekame dezes stumima grid based style
    void OnCollisionEnter2D(Collision2D other)
    {
        BoxConundrum box = other.transform.GetComponent<BoxConundrum>(); // gaunama kito objekto pozicija jeigu tas objektas turi "BoxConundrum" skripta kitaip null

        //jei objektas deze(stumiamas objektas) atliekame
        if (box != null)
        {
            //Atliekamas kito objekto metodas "BoxPush" kur paduodama kryptis i kuria stumti ir "blockingLayer" lapas kuriame tikrins ar yra i ka galetu atsitrenkti
            box.BoxPush(direction, blockingLayer);
        }
    }
    
    
    // FixedUpdate yra naudojamas fizikai zaidime yra iskvieciamas pastoviai kol zaidimas veikia 
    //Judejimas per komponenta Rigidbody2D blokuojamas istrizas judejimas
    private void FixedUpdate()
    {
        if (direction.x != 0)
            direction.y = 0;
        if (direction.x != 0 || direction.y != 0)
        {
            //rb.position - dabartine vieta objekto
            //direction - kryptis i kuria judinsime objekta (turi x ir y, pvz x=1 ir y=0, tai judes i desine)
            //wormSpeed - greitis, kaip greitai nusigauna i norima taska
            //Time.deltaTime - laikrodis butinas judejimui kad nebutu nesamoniu 
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);//Naudojamas Unity metodas "MovePosition"  judinti zaidejui
            rb.velocity = Vector2.zero; //isjungiamas pagreitis kad nebutu nesamoniu (pastumiamas zaidejas judancio objekto igauna begalini pagreiti)
        }

    }
}
