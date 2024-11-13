using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace VSVR_Panorama
{

    public class PanoramaManager : MonoBehaviour
    {
        public static PanoramaManager Instance;
        [SerializeField]
        private Material material_pano;
        [SerializeField]
        private Material material_panoFade;
        [SerializeField]
        private Material material_building;
        [SerializeField]
        private Shader shader_fadeAway;
        [SerializeField]
        private Material material_pojector_building;
        private void Awake()
        {
            Instance = this;
        }
        public Material GetPanoramaMaterial()
        {
            return material_pano;
        }
        public Material GetPanoramaFadeMaterial()
        {
            return material_panoFade;
        }
        public Material GetBuildingMaterial()
        {
            return material_building;
        }
        public Material GetPojectorBuildingMaterial()
        {
            return material_pojector_building;
        }
        public Shader GetShaderFadeAway()
        {
            return shader_fadeAway;
        }
    }

}