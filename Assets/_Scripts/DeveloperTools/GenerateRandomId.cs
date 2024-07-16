using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomId
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}
