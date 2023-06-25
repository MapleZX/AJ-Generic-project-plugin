using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "AJ Custom Rule Tile", menuName = "AJ Generic Tools/AJ Custom Rule Tile", order = 2)]
public class AJGameCustomRuleTile : RuleTile<AJGameCustomRuleTile.Neighbor> {
    public TileRuleStyle siblingGroup;
    public class Neighbor : RuleTile.TilingRule.Neighbor {
        // public const int Sibing = 3;
    }
    public override bool RuleMatch(int neighbor, TileBase other) {       
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other is AJGameCustomRuleTile
                        && (other as AJGameCustomRuleTile).siblingGroup == this.siblingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is AJGameCustomRuleTile
                        && (other as AJGameCustomRuleTile).siblingGroup == this.siblingGroup);
                }
        }
        return base.RuleMatch(neighbor, other);
    }
}