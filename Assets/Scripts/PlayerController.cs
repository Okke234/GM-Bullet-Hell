using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player p;
    private float horInput, verInput = 0f;
    private float fdt;

    void Start()
    {
        p = Player.Instance;
        fdt = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        horInput = Input.GetAxisRaw("Horizontal") * p.speed;
        verInput = Input.GetAxisRaw("Vertical") * p.speed;
    }

    void FixedUpdate()
    {
        if (horInput != 0 || verInput != 0)
        {
            p.Move(horInput * fdt, verInput * fdt);
        }
    }
}
