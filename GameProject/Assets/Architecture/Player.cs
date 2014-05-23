using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public abstract class Player
{

    // VARIABLES
    public Car Car { get; set; }

    // UNITY VARIABLES
    public GameObject CarObject = null;

    /**
     * Returns the function of the current player as type of class.
     */
    public Type getFunction()
    {
        return this.GetType();
    }

    /**
     * Returns the function of the current player as a String.
     */
    public string getFunctionName()
    {
        return this.GetType().Name;
    }
}
