using UnityEngine;

namespace Tyrant
{
    public interface ILog
    {
        public string title { get; }
        
        public string message { get; }
    }
    
    public class TLog
    {

        public static void Log(ILog logObject)
        {
            Debug.Log($"#{logObject.title}# {logObject.message}");
        }
        
    }
    
    
    
    
}