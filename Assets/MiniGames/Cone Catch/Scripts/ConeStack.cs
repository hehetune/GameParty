using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniGames.Cone_Catch.Scripts
{
    public class ConeStack : MonoBehaviour
    {
        [SerializeField] private List<Cone> cones = new();
        [SerializeField] private float swipeSpeed = 2f;
        
        public void AttachCone(Cone cone)
        {
            cone.ConeStack = this;
            cones.Add(cone);
        }

        private void Update()
        {
            if (cones.Count > 1)
            {
                for (int i = 1; i < cones.Count; i++)
                {
                    var FirstCone = cones.ElementAt(i - 1);
                    var SectCone = cones.ElementAt(i);

                    //  var DesireDistance = Vector3.Distance(FirstCone.position,SectCone.position );

                    //    if (DesireDistance <= Distance)
                    //    {
                    SectCone.transform.position = new Vector3(Mathf.Lerp(SectCone.transform.position.x,FirstCone.transform.position.x,swipeSpeed * Time.deltaTime)
                        ,SectCone.transform.position.y,Mathf.Lerp(SectCone.transform.position.z,FirstCone.transform.position.z + 0.5f,swipeSpeed * Time.deltaTime));
                    //  }
                }
        
            }
        }
    }
}