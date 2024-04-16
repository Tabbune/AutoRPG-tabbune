using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAddressable
{
    bool isEnemy { get; set; }
    string className { get; set; }
}
