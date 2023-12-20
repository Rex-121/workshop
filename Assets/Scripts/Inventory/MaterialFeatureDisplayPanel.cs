using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Tyrant
{
    public class MaterialFeatureDisplayPanel: MonoBehaviour
    {
        public Image icon;

        public MaterialFeature[] materialFeature
        {
            get => _materialFeature;
            set
            {
                _materialFeature = value;
                Refresh();
            }
        }

        private MaterialFeature[] _materialFeature;

        private void Refresh()
        {
            if (!_materialFeature.Any()) return;
            icon.sprite = materialFeature.First().icon;
        }
        
        
    }
}