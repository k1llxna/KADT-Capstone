using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviourPun
{
    protected CharacterController controller;
    protected Animator animator;
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed; // Used when not using MouseLook.CS to rotate character
    public float gravity;
    public bool isGrounded;

    public float health;
    public int maxMana;
    protected int mana = 0;

    protected Vector3 moveDirection = Vector3.zero;

    public GameObject[] towers;
    public Ability[] abilities;
    protected bool building;

    protected enum ControllerType {  SimpleMove, Move };
    [SerializeField] protected ControllerType type;

    protected int maxMoney = 100;
    public int money;

    protected float manaRefreshRate = 0;
    protected int manaRefreshCooldown = 3; //How long till recharge
    protected float manaRefreshTime = 0; //Time till cooldown is over
    protected int manaGain = 1; //How much per Tick

    // Start is called before the first frame update
    void Start()
    {
        //Always start with mana regen
        manaRefreshTime = 3;
        mana = 20;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Physics.IgnoreLayerCollision(8, 8, true);

        if (speed <= 0)
        {
            speed = 6.0f;
            Debug.Log("Speed not set on " + name + " defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;
            Debug.Log("JumpSpeed not set on " + name + " defaulting to " + jumpSpeed);
        }

        if (rotationSpeed <= 0)
        {
            rotationSpeed = 10.0f;
            Debug.Log("RotationSpeed not set on " + name + " defaulting to " + rotationSpeed);
        }

        if (gravity <= 0)
        {
            gravity = 9.81f;
            Debug.Log("Gravity not set on " + name + " defaulting to " + gravity);
        }

        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        Move();

        if (!building)
        {
            if (Input.GetKeyDown("1"))
                StartCoroutine(Building(0));
            else if (Input.GetKeyDown("2"))
                StartCoroutine(Building(1));
            else if (Input.GetKeyDown("3"))
                StartCoroutine(Building(2));
            else if (Input.GetKeyDown("4"))
                StartCoroutine(Building(3));

            else if (Input.GetKeyDown("5"))
                abilities[0].Use(this);
            else if (Input.GetKeyDown("6"))
                StartCoroutine(Building(1));
            else if (Input.GetKeyDown("7"))
                StartCoroutine(Building(2));
            else if (Input.GetKeyDown("8"))
                StartCoroutine(Building(3));


            if (Input.GetMouseButtonDown(0))
                Attack();

            //Get information of targeted object with raycast
            Target();
        }

        if(controller.isGrounded)
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }

        UpdateMana();
    }

    IEnumerator Building(int towerNum)
    {
        OTower tower = towers[towerNum].GetComponent<OTower>();

        //Break if we dont have enough money
        if (money < tower.cost)
        {
            Debug.Log("Dont have enough mana to build tower");
            //Play error sound
        }
        else
        {
            //We can only raycast to the ground
            LayerMask ground = 1 << 9;

            bool hasBuilt = false;
            building = true;

            //This would be a tempTower array, and will instantiate the real tower later
            GameObject towerObject = Instantiate(towers[0], new Vector3(100, 100, 100), transform.rotation);

            RaycastHit hit;
            Vector3 offset = new Vector3(0, 1, 0);
            Vector3 towerOffset = new Vector3(0, 0.5f, 0);

            while (!hasBuilt)
            {
                Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 5, Color.red);

                if (Physics.Raycast(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 5, ground))
                {
                    towerObject.transform.position = hit.point + towerOffset;
                    towerObject.transform.rotation = transform.rotation;
                }

                if (Input.GetMouseButton(0))
                {
                    print("finished building");
                    money -= tower.cost;
                    hasBuilt = true;                    
                }

                yield return new WaitForEndOfFrame();
            }

            //While(!hasTurned)
            //allow option for fine rotatating of the newly built tower

            towerObject.GetComponent<OTower>().Placed();
            building = false;
        }
    }

    protected virtual void Move()
    {
        switch (type)
        {
            case ControllerType.SimpleMove:
                controller.SimpleMove(transform.forward * (Input.GetAxis("Vertical") * speed));
                break;

            case ControllerType.Move:
                if (isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);

                    moveDirection *= speed;

                    if (Input.GetButtonDown("Jump"))
                    {
                        moveDirection.y = jumpSpeed;
                        isGrounded = false;
                        animator.SetBool("Grounded", false);
                    }

                }
                else
                {
                    float tempY = moveDirection.y;

                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                    moveDirection = transform.TransformDirection(moveDirection);

                    moveDirection *= speed;

                    moveDirection = new Vector3(moveDirection.x, tempY, moveDirection.z);

                    moveDirection.y -= gravity * Time.deltaTime;
                }


                controller.Move(moveDirection * Time.deltaTime);

                animator.SetFloat("Speed", Mathf.Abs(moveDirection.z));

                break;
        }



        float yLerp = Mathf.LerpAngle(transform.eulerAngles.y, Camera.main.transform.eulerAngles.y, 0.05f);

        Quaternion newRotation = Quaternion.Euler(transform.rotation.x, yLerp, transform.rotation.z);

        transform.rotation = newRotation;

    }

    protected virtual void Attack(){ }

    protected void UpdateMana()
    {
        //If we are within the cooldown time increase mana
        if(manaRefreshTime >= manaRefreshCooldown)
        {
            //Only increase if the tick is over and we are not full mana
            if (manaRefreshRate >= 0.4f && mana < maxMana)
            {
                mana += manaGain;
                manaRefreshRate = 0;
            }
            else
            {   
                //Add time to tick rate
                manaRefreshRate += Time.deltaTime;
            }
        }
        else
        {
            //Add time to cooldown
            manaRefreshTime += Time.deltaTime;
        }
    }

    protected virtual void Target()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 1, 0);

        if (Physics.Raycast(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag.Equals("Enemy"))
            {
                UIHandler uihandler = hit.transform.gameObject.GetComponent<UIHandler>();
                uihandler.StopAllCoroutines();
                uihandler.StartCoroutine("ShowUI");

                //Keeps HealthBar Roughly the same size throughout
                float size = (transform.position - hit.transform.position).magnitude/100;
                if (size < 0.08)
                    size = 0.08f;

                uihandler.slider.transform.localScale = new Vector3(size, size, size);

                Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            }

            if (hit.transform.tag.Equals("Tower"))
            {
                TowerUIController uihandler = hit.transform.gameObject.GetComponent<TowerUIController>();
                OTower tower = hit.transform.gameObject.GetComponent<OTower>();

                if (Mathf.Abs((transform.position - hit.transform.position).magnitude) <= 10)
                {                     
                    uihandler.StopAllCoroutines();
                    uihandler.StartCoroutine("ShowUI");

                    //Keeps HealthBar Roughly the same size throughout
                    float size = (transform.position - hit.transform.position).magnitude / 100;
                    if (size < 0.08)
                        size = 0.08f;

                    uihandler.panel.transform.localScale = new Vector3(size, size, size);

                    Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                }

                if(Input.GetKeyDown("g"))
                {
                    StartCoroutine(UpgradeBuilding(tower, uihandler));
                }

                else if(Input.GetKeyDown("e"))
                {
                    StartCoroutine(RepairBuilding(tower));
                }

                else if(Input.GetKeyDown("v"))
                {
                    StartCoroutine(SellBuilding(tower, uihandler));
                }

            }
        }
        else
        {
            Debug.DrawRay(transform.position + offset, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, Color.yellow);
        }
    }

    protected IEnumerator UpgradeBuilding(OTower tower, TowerUIController UIController)
    {
        float timeElapsed = 0;
        float timeToUpgrade = tower.level * 2;

        UIController.upgradeBar.maxValue = timeToUpgrade;
        UIController.upgradeBar.gameObject.SetActive(true);

        while (Input.GetKey("g"))
        {
            timeElapsed += Time.deltaTime;

            UIController.upgradeBar.value = timeElapsed;

            if (timeElapsed >= timeToUpgrade)
            {
                tower.Upgrade();
                UIController.upgradeBar.value = 0;
                UIController.upgradeBar.gameObject.SetActive(false);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        UIController.upgradeBar.value = 0;
        UIController.upgradeBar.gameObject.SetActive(false);
    }

    protected IEnumerator SellBuilding(OTower tower, TowerUIController UIController)
    {
        float timeElapsed = 0;
        float timeToUpgrade = 2;

        UIController.sellBar.maxValue = timeToUpgrade;
        UIController.sellBar.gameObject.SetActive(true);

        while (Input.GetKey("v"))
        {
            timeElapsed += Time.deltaTime;

            UIController.sellBar.value = timeElapsed;

            if (timeElapsed >= timeToUpgrade)
            {
                tower.Sell(this);
                UIController.sellBar.value = 0;
                UIController.sellBar.gameObject.SetActive(false);
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        UIController.sellBar.value = 0;
        UIController.sellBar.gameObject.SetActive(false);
    }

    protected IEnumerator RepairBuilding(OTower tower)
    {
        while (Input.GetKey("e"))
        {
            tower.Repair();
            money -= 1;

            if (tower.health == tower.maxHealth)
            {
                break;
            }

            //Cant repair too fast
            yield return new WaitForSeconds(0.1f);
        }
    }


    //Take damage based on damage that was dealt
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void GiveMoney(int amount)
    {
        money += amount;
    }

    public int GetMoney()
    {
        return money;
    }

    public int GetMana()
    {
        return mana;
    }

    public void UseMana(int cost)
    {
        mana -= cost;
        manaRefreshTime = 0;
    }
}
