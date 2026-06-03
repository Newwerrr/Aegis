using UnityEngine;

namespace Aegis
{
    public class AegisRunner : MonoBehaviour
    {
        void Update()
        {
            AegisRuntime.ProcessQueue();
            AegisRuntime.Update();
        }

        void LateUpdate()
        {
            AegisRuntime.Render();
        }
    }
}