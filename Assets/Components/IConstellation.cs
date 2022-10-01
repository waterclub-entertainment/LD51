using System;
using System.Collections.Generic;

public interface IConstellation {

	[Serializable]
	public class Connection
	{
	    public int from, to;
	}

    public class ConnectionComparer : IEqualityComparer<Connection>
    {

        bool IEqualityComparer<Connection>.Equals(Connection x, Connection y)
        {
            return (x.from == y.from && x.to == y.to) || (x.from == y.to && x.to == y.from);
        }

        int IEqualityComparer<Connection>.GetHashCode(Connection obj)
        {
			return Math.Min(obj.from, obj.to) * 1000 + Math.Max(obj.from, obj.to);
        }
    }

    // Return true iff star is used in this constellation
    public bool ContainsStar(int id);
	
	// Return all connections
	public Connection[] GetConnections();
	
	// Return true iff there is a connection between stars with ids `from` and `to`
	// (or the other way round)
	public bool HasConnection(int from, int to);
}
