using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamRelation { Friendly, Hostile, Neutral}
public interface ITeamInterface 
{
    public int GetTeamID() { return -GetNeutralTeamID(); }
    public int GetNeutralTeamID() {  return -1; }

    TeamRelation GetRelationTowards(GameObject other)
    {
        ITeamInterface otherInterface = other.GetComponent<ITeamInterface>();
        if(otherInterface == null) return TeamRelation.Neutral;

        if(otherInterface.GetTeamID() == GetNeutralTeamID() || GetTeamID() == GetNeutralTeamID())
        {
            return TeamRelation.Neutral;
        }

        if(otherInterface.GetTeamID() == GetTeamID())
        {
            return TeamRelation.Friendly;
        }

        return TeamRelation.Hostile;

    }


}
