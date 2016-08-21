using UnityEngine;
using System.Collections;

public class ParticlesController : MonoBehaviour
{
    public GameController game;

	void Start ()
	{
        m_particles = GetComponent<ParticleSystem>();
	}
	
	void Update ()
	{
        ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[m_particles.particleCount];
        m_particles.GetParticles(_particles);

        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].velocity = new Vector3(_particles[i].velocity.x, _particles[i].velocity.y, game.GetGameSpeed() * 2.0f);
        }

        m_particles.SetParticles(_particles, _particles.Length);
	}

    ParticleSystem m_particles;
}
