using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager
{
    public GameManager gameManager;
    public Manager()
    {
        gameManager = GameManager.instance;
    }
    public virtual void Awake()
    { 
    
    }
    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }
}
