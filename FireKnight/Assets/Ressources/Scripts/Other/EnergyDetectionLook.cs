using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Enemies;
using System;

namespace Dim
{
    public class EnergyDetectionLook : MonoBehaviour
    {

        LaserTurret laserTurret;
        ParticleSystem ps;
        GameObject grayscaleObject;
        GameObject LightbulbObject;
        ParticleSystem grayscalePs;
        new Light light;

        public bool isVisible;

        [SerializeField] Color color;
        [SerializeField] Material lightcone;
        [SerializeField] Material graycone;

        Transform LaserTurretPosiiton;

        void Start()
        {

            isVisible = true;

            laserTurret = GetComponent<LaserTurret>();

            LaserTurretPosiiton = transform;
            while (LaserTurretPosiiton.name != "Tip")
            {
                LaserTurretPosiiton = LaserTurretPosiiton.GetChild(0);
            }

            //light = LightSetup();
            LightbulbObject = new GameObject();

            LightbulbObject.transform.parent = transform;

            LightbulbObject.transform.position = LaserTurretPosiiton.position;
            LightbulbObject.transform.localRotation = Quaternion.identity;
            LightbulbObject.transform.localScale = Vector3.one;
            ps = ParticleSystemSetup();

            grayscaleObject = new GameObject();
            
            grayscaleObject.transform.parent = transform;

            grayscaleObject.transform.position = LaserTurretPosiiton.position;
            grayscaleObject.transform.localRotation = Quaternion.identity;
            grayscaleObject.transform.localScale = Vector3.one;

            grayscalePs = GrayScaleSystemSetup();
        }

        ParticleSystem ParticleSystemSetup()
        {
            ParticleSystem ps = LightbulbObject.AddComponent<ParticleSystem>();

            if (laserTurret != null)
            {
                var main = ps.main;
                main.prewarm = true;
                main.startSpeed = 0;
                main.startLifetime = 1;
                main.simulationSpace = ParticleSystemSimulationSpace.Local;
                main.startColor = color;

                main.startSize3D = true;
                main.startSizeX = 2.5f * laserTurret.DetectionBaseRadius;
                main.startSizeY = laserTurret.DetectionRange;

                main.startRotation3D = true;
                main.startRotationY = 1.570796313445772f;
                //main.startRotationZ = -1.570796313445772f;

                var emission = ps.emission;
                emission.rateOverTime = 1;

                var shape = ps.shape;
                shape.enabled = false;

                var textureSheetAnimation = ps.textureSheetAnimation;
                textureSheetAnimation.enabled = true;
                textureSheetAnimation.numTilesX = 8;
                textureSheetAnimation.numTilesY = 4;

                ParticleSystemRenderer psr = LightbulbObject.GetComponent<ParticleSystemRenderer>();
                psr.material = lightcone;
                psr.alignment = ParticleSystemRenderSpace.Local;
                psr.pivot = Vector3.down* 0.5f;
                psr.maxParticleSize = 100;
            }

            return ps;
        }

        public void UpdateVisibility(bool isActive)
        {
            LightbulbObject.SetActive(isActive);
            grayscaleObject.SetActive(isActive);

            isVisible = isActive;
        }

        ParticleSystem GrayScaleSystemSetup()
        {
            ParticleSystem ps = grayscaleObject.AddComponent<ParticleSystem>();

            if (laserTurret != null)
            {
                var main = ps.main;
                main.prewarm = true;
                main.startSpeed = 0;
                main.startLifetime = 1;
                main.simulationSpace = ParticleSystemSimulationSpace.Local;
                main.startColor = color;

                main.startSize3D = true;
                main.startSizeX = 2.5f * laserTurret.DetectionBaseRadius;
                main.startSizeY = laserTurret.DetectionRange;

                main.startRotation3D = true;
                main.startRotationY = 1.570796313445772f;
                //main.startRotationZ = -1.570796313445772f;

                var emission = ps.emission;
                emission.rateOverTime = 1;

                var shape = ps.shape;
                shape.enabled = false;

                var textureSheetAnimation = ps.textureSheetAnimation;
                textureSheetAnimation.enabled = true;
                textureSheetAnimation.numTilesX = 8;
                textureSheetAnimation.numTilesY = 4;

                ParticleSystemRenderer psr = grayscaleObject.GetComponent<ParticleSystemRenderer>();
                psr.material = graycone;
                psr.alignment = ParticleSystemRenderSpace.Local;
                psr.pivot = Vector3.down* 0.5f;
                psr.maxParticleSize = 100;
            }

            return ps;
        }

        Light LightSetup()
        {
            Light l = gameObject.AddComponent<Light>();
            l.type = LightType.Spot;
            l.color = color;
            l.renderMode = LightRenderMode.ForcePixel;

            if (laserTurret != null)
            {
                l = LaserTurretSetup(l);
            }

            return l;
        }

        Light LaserTurretSetup(Light l)
        {
            l.range = laserTurret.DetectionRange + 5;
            l.spotAngle = laserTurret.DetectionBaseRadius * 7.5f;
            l.intensity = laserTurret.DetectionBaseRadius;

            return l;
        }
    }
}
