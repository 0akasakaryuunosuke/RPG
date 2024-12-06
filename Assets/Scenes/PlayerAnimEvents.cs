using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Playercs player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Playercs>();
    }

    // Update is called once per frame
    private void AnimationTrigger()
    {
        player.AttackOver();
    }
    
}
