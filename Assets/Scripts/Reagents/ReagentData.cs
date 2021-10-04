using TirUtilities.Extensions;
using UnityEngine;

namespace LudumDare49
{
    ///<!--
    /// ReagentData.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 02, 2021
    /// Updated:  Oct 02, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Reagent Data")]
    public class ReagentData : ScriptableObject/*, System.IEquatable<ReagentData>*/
    {
        #region Inspector Fields

        [SerializeField] private string _name = string.Empty;

        #endregion

        #region Public Properties

        public string Name => _name;

        #endregion

        #region Equality

        // This isn't needed
        //public bool Equals(ReagentData other) =>
        //    !other.IsNull() && (ReferenceEquals(this, other) 
        //    || (GetType() == other.GetType() && other.Name.Equals(Name)));

        //public static bool operator ==(ReagentData lhs, ReagentData rhs) =>
        //    lhs.IsNull() ? rhs.IsNull() : lhs.Equals(rhs);

        //public static bool operator !=(ReagentData lhs, ReagentData rhs) => !(lhs == rhs);

        //public override bool Equals(object other) => other is ReagentData otherData && Equals(otherData);
        //public override int GetHashCode() => base.GetHashCode();

        #endregion
    }
}