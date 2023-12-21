using UnityEngine;

namespace Tyrant
{
    public class HeroRequest: MonoBehaviour
    {
        
        

        public void PreviewBluePrint()
        {
            
            var random = Random.Range(0, 2);
            var so = RequestGenesis.main.bluePrintSO[random];
            
            RequestManager.main.PreviewBluePrint(BluePrint.FromSO(so));
            
        }
        
    }
}