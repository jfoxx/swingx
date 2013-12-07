using UnityEngine;
using System.Collections;

public class GrappleLine : MonoBehaviour
{

	
	public Transform player;
	public Transform grapple;
	public LineRenderer line;

	void Start ()
	{

		line = GetComponent<LineRenderer> ();
		line.SetWidth (0.2f, 0.2f);

		if(!networkView.isMine){return;}

		player = transform.parent;
		grapple = transform.parent.transform.FindChild("GrapplePoint_mp(Clone)");

	}

	float interpolationBackTime = 0.1f;
	class LineState
	{
		public float timeStamp;
		public Vector3 plPos;
		public Vector3 grPos;
	}
	private LineState[] m_BufferedState = new LineState[20]; // where we store the State of our controller
	private int m_TimestampCount; // keeps track of what slots are used
	
	void Update ()
	{
//		if(networkView.isMine){
//			line.SetPosition (0, player.transform.position);
//			line.SetPosition (1, grapple.transform.position);
//		}

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
						LineState rhs = m_BufferedState[Mathf.Max(i-1, 0)];
						// The best playback state (closest to 100 ms old (default time))
						LineState lhs = m_BufferedState[i];
						
						// Use the time between the two slots to determine if interpolation is necessary
						float length = rhs.timeStamp - lhs.timeStamp;
						float t = 0.0f;
						
						if (length > 0.0001)
						{
							t = ((interpolationTime - lhs.timeStamp) / length);
						}
						
						// if t=0 => lhs is used directly
						line.SetPosition (0, Vector3.Lerp(lhs.plPos, rhs.plPos, t));
						line.SetPosition (1, Vector3.Lerp(lhs.grPos, rhs.grPos, t));
						return;
						
					}
				}
			}
			else
			{
				Debug.Log("networkview mine");

				if (m_BufferedState[0] != null)
				{
					if(grapple != null && player != null){
						LineState latest = m_BufferedState[0];
						player.transform.position = latest.plPos;
						grapple.transform.position = latest.grPos;
					}
				}
			}
		}
	}

	void OnSerializeNetworkView(BitStream stream,NetworkMessageInfo info)
	{
		Vector3 playerPos;
		Vector3 grapplePos;

		if (stream.isWriting)
		{
			playerPos = player.transform.position;
			grapplePos = grapple.transform.position;
			
			stream.Serialize(ref playerPos);
			stream.Serialize(ref grapplePos);
		}
		else
		{
			playerPos = Vector3.zero;
			grapplePos = Vector3.zero;
			stream.Serialize(ref playerPos);
			stream.Serialize(ref grapplePos);
			
			
			for (var i = m_BufferedState.Length - 1; i >= 1; i --)
			{
				m_BufferedState[i] = m_BufferedState[i-1];
			}
			
			LineState state = new LineState();
			state.timeStamp = (float)info.timestamp;
			state.plPos = playerPos;
			state.grPos = grapplePos;
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
