using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {

    [Header("First is always playing, second is playing after activation")]
    public ParticleSystem firstParticleSys;
    public ParticleSystem secParticleSys;

    ParticleSystem.MinMaxCurve speed;

    // Use this for initialization
    void Start () {
        firstParticleSys.Play();

        speed = secParticleSys.main.startSpeed;

        secParticleSys.Stop();
        secParticleSys.Clear();
	}

    public void StartSecParticleSys()
    {
        var p = secParticleSys.main;
        p.simulationSpeed = speed.constant;

        secParticleSys.Play();
    }

    public void StopSecParticleSys()
    {
        var p = secParticleSys.main;
        p.simulationSpeed = 10f;

        secParticleSys.Stop();
    }
}
