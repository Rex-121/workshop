using UnityEngine;

namespace Tyrant
{
    [CreateAssetMenu(menuName = "Singleton/RequestGenesis", fileName = "RequestGenesis")]
    public class RequestGenesis: SingletonSO<RequestGenesis>
    {

        public BluePrintSO[] bluePrintSO;

    }
}