using System;

public interface IConstellation {

	[Serializable]
	public class Connection
	{
	    public int from, to;
	}

	// Return true iff star is used in this constellation
	public bool ConstainStar(int id);
	
	// Return all connections
	public Connection[] GetConnections();
	
	// Return true iff there is a connection between stars with ids `from` and `to`
	// (or the other way round)
	public bool HasConnection(int from, int to);
}
