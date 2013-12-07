using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
	public Transform target;
	public int zigs = 100;
	public float speed = 1f;
	public float scale = 1f;

	Perlin noise;
	float oneOverZigs;
	Vector3 _targetPosition;

	private Particle[] particles;

	float interpolationBackTime = 0.1f;

	class LightningState
	{
		public float timeStamp;
		public Vector3 pos;
		public Vector3 targetPos;
	}

	private LightningState[] m_BufferedState = new LightningState[20]; // where we store the State of our controller
	private int m_TimestampCount; // keeps track of what slots are used

	
	void Start()
	{
		oneOverZigs = 1f / (float)zigs;
		particleEmitter.emit = false;

		particleEmitter.Emit(zigs);
		particles = particleEmitter.particles;
	}

	void Update ()
	{
		if(networkView.isMine){
			_targetPosition = target.transform.position;
		}

		if (noise == null)
			noise = new Perlin();
		
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;
		
		for (int i=0; i < particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, _targetPosition, oneOverZigs * (float)i);
			Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
			                             noise.Noise(timey + position.x, timey + position.y, timey + position.z),
			                             noise.Noise(timez + position.x, timez + position.y, timez + position.z));
			position += (offset * scale * ((float)i * oneOverZigs));
			
			particles[i].position = position;
			particles[i].color = Color.white;
			particles[i].energy = 1f;
		}
		
		particleEmitter.particles = particles;


		if (!networkView.isMine)
		{      
			float currentTime = (float)Network.time;
			float interpolationTime = currentTime - interpolationBackTime;
			
			if (m_BufferedState[0] != null && m_BufferedState[0].timeStamp > interpolationTime)
			{
				for (var i = 0; i < m_TimestampCount; i++)
				{
					if (m_BufferedState[i].timeStamp <= interpolationTime || i == m_TimestampCount - 1)
					{
						// The state one slot newer (<100ms) than the best playback state
						LightningState rhs = m_BufferedState[Mathf.Max(i-1, 0)];
						// The best playback state (closest to 100 ms old (default time))
						LightningState lhs = m_BufferedState[i];
						
						// Use the time between the two slots to determine if interpolation is necessary
						float length = rhs.timeStamp - lhs.timeStamp;
						float t = 0.0f;
						
						if (length > 0.0001)
						{
							t = ((interpolationTime - lhs.timeStamp) / length);
						}
						
						// if t=0 => lhs is used directly
						transform.position = Vector3.Lerp(lhs.pos, rhs.pos, t);
						_targetPosition = Vector3.Lerp(lhs.targetPos, rhs.targetPos, t);
						return;
						
					}
				}
			}
			else
			{
				
				if (m_BufferedState[0] != null)
				{
					if(target != null){
						LightningState latest = m_BufferedState[0];
						transform.position = latest.pos;
						target.transform.position = latest.targetPos;
					}
				}
			}
		}
	}
	
	void OnSerializeNetworkView(BitStream stream,NetworkMessageInfo info)
	{
		Vector3 pos;
		Vector3 targetPos;
		
		if (stream.isWriting)
		{
			pos = transform.position;
			targetPos = target.transform.position;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref targetPos);
		}
		else
		{
			pos = Vector3.zero;
			targetPos = Vector3.zero;
			stream.Serialize(ref pos);
			stream.Serialize(ref targetPos);
			
			
			for (var i = m_BufferedState.Length - 1; i >= 1; i --)
			{
				m_BufferedState[i] = m_BufferedState[i-1];
			}
			
			LightningState state = new LightningState();
			state.timeStamp = (float)info.timestamp;
			state.pos = pos;
			state.targetPos = targetPos;
			m_BufferedState[0] = state;
			
			m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);
			
			for (int i = 0; i < m_TimestampCount-1; i++)
			{
				if (m_BufferedState[i].timeStamp < m_BufferedState[i+1].timeStamp)
				{
					Debug.Log("State inconsistent");
				}
			}
		}
	}
	
	void OnDestroy(){
		if(networkView.isMine){
			Network.RemoveRPCs(networkView.viewID);
		}
	}
}