using System;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public class WaitForObjectActive : CustomYieldInstruction
    {
        Func<bool> m_Predicate;
        public override bool keepWaiting {
            get {
                return !m_Predicate();
            }
        }
        public WaitForObjectActive(params object[] objs)
        {
            m_Predicate = () => { 
                foreach (var obj in objs) {
                    if (obj == null)
                        return true;
                } 
                return false; 
            };
        }
    }
}
