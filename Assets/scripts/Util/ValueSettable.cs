using UnityEngine;
using System.Collections;

/** 
 * An interface for objects that can consume a floating point value. 
 */

public interface ValueSettable
{
    void SetValue(float value);
}
