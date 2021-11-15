using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public enum Currency
    {
        None,
        Wood,
        Stone,
        Food,
    }
}
