using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinnigOBJ : MonoBehaviour
{


    public void Start()
    {
        StartCoroutine(SpinObject());
    }
    IEnumerator SpinObject()
    {
        while (true)
        {
            for (int th = 0; th < 360; th++)
            {
                var rad = Mathf.Deg2Rad * th;
                var x = 40  * Mathf.Sin(rad);
                var z = 40 * Mathf.Cos(rad);
                this.transform.position = new Vector3(x, 0,z);
                yield return new WaitForSeconds(0.0378f);
            }
        }
      
    }
}
