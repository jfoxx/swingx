using UnityEngine;
using System.Collections;

public class ClientPrediction : MonoBehaviour {
	
	float interpolationBackTime = 0.1f;
	class State
	{
		public float timeStamp;
		public Vector3 pos;
		public Quaternion rot;
	}
	private State[] m_BufferedState = new State[20]; // where we store the State of our controller
	private int m_TimestampCount; // keeps track of what slots are used

	void OnSerializeNetworkView(BitStream stream,NetworkMessageInfo info)
	{
		Vector3 pos;
		Quaternion rot;
		if (stream.isWriting)
		{
			pos = transform.position;
			rot = transform.rotation;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else
		{
			pos = Vector3.zero;
			rot = Quaternion.identity;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			
			
			for (var i = m_BufferedState.Length - 1; i >= 1; i --)
			{
				m_BufferedState[i] = m_BufferedState[i-1];
			}
			
			State state = new State();
			state.timeStamp = (float)info.timestamp;
			state.pos = pos;
			state.rot = rot;
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
	
	void Update ()
	{
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
						State rhs = m_BufferedState[Mathf.Max(i-1, 0)];
						// The best playback state (closest to 100 ms old (default time))
						State lhs = m_BufferedState[i];
						
						// Use the time between the two slots to determine if interpolation is necessary
						float length = rhs.timeStamp - lhs.timeStamp;
						float t = 0.0f;
						
						if (length > 0.0001)
						{
							t = ((interpolationTime - lhs.timeStamp) / length);
						}
						
						// if t=0 => lhs is used directly
						transform.position = Vector3.Lerp(lhs.pos, rhs.pos, t);
						transform.rotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
						return;
						
					}
				}
			}
			else
			{
				if (m_BufferedState[0] != null)
				{
					State latest = m_BufferedState[0];
					transform.position = latest.pos;
					transform.rotation = latest.rot;
				}
			}
		}
	}
}