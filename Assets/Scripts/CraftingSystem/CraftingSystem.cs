using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TirUtilities.Extensions;
using UnityEngine;

namespace LudumDare49.Crafting
{
    ///<!--
    /// CraftingSystem.cs
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
    public class CraftingSystem : SerializedMonoBehaviour
    {
        #region Data Structures

        [System.Serializable]
        public struct BombType
        {
            [HorizontalGroup("BombType")]
            [SerializeField, FoldoutGroup("BombType/$Name")] 
            private string _name;
            [SerializeField, FoldoutGroup("BombType/$Name"), HideLabel] 
            private Recipe _recipe;
            [SerializeField, TableColumnWidth(57, resizable: false), AssetsOnly,
                PreviewField(Alignment = ObjectFieldAlignment.Center)]
            private GameObject _prefab;

            public string Name => _name;
            public Recipe Recipe => _recipe;
            public GameObject Prefab => _prefab;
        }

        [System.Serializable]
        public struct Recipe 
        {
            [SerializeField, ValueDropdown(nameof(GetReagentData))]
            private List<ReagentData> _reagents;
            public IReadOnlyList<ReagentData> Reagents
            {
                get
                {
                    _reagents.Sort((i , j) => i.Name.CompareTo(j.Name));
                    return _reagents;
                }
            }

            public static IEnumerable GetReagentData() => ReagentData.GetAllReagentData();
        }

        #endregion

        #region Inspector Fields

        [SerializeField, TableList(NumberOfItemsPerPage = 4)] 
        private List<BombType> _bombTypes = new List<BombType>();
        [SerializeField] private List<StorageZone> _storageZones = new List<StorageZone>();
        [SerializeField] private Transform _spawnPoint;

        #endregion

        #region Private Fields

        private Dictionary<IReadOnlyList<ReagentData>, BombType> _recipeBook =
            new Dictionary<IReadOnlyList<ReagentData>, BombType>(new ListComparer());
        private BombType _selectedBomb;

        private List<ReagentData> _selectedReagents = new List<ReagentData>();
        public List<ReagentData> SelectedReagents => _selectedReagents;

        private GameObject _bombObject;

        #endregion

        #region Unity Messages

        private void Awake() => BuildRecipeBook();

        #endregion

        #region Methods

        private void BuildRecipeBook()
        {
            foreach (var bomb in _bombTypes)
                _recipeBook[bomb.Recipe.Reagents] = bomb;

            _selectedBomb = _bombTypes[0];
        }

        public void LookupRecipe()
        {
            _selectedReagents.Sort((i, j) => i.Name.CompareTo(j.Name));
            if (_recipeBook.TryGetValue(_selectedReagents, out _selectedBomb))
                CreateBomb();
        }

        public void CreateBomb()
        {
            if (_bombObject.NotNull()) Destroy(_bombObject);

            var inst = Instantiate(_selectedBomb.Prefab);
            inst.transform.position = _spawnPoint.position;
            inst.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            inst.GetComponent<Bomb>().SetTransparent();
            _bombObject = inst;
        }

        public void CraftBomb()
        {
            if (_bombObject.IsNull())
                return;

            _bombObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _bombObject.GetComponent<Bomb>().SetOpaque();
            foreach (var reagent in _selectedReagents)
            {
                var found = _storageZones.Where(z => z.BoundReagentType == reagent);
                found.ToList().ForEach(zone => zone.ReturnReagent());
            }
            _selectedReagents.Clear();
        }

        #endregion
    }

    class ListComparer : IEqualityComparer<IReadOnlyList<ReagentData>>
    {
        public bool Equals(IReadOnlyList<ReagentData> x, IReadOnlyList<ReagentData> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IReadOnlyList<ReagentData> obj)
        {
            int hashcode = 0;
            foreach (ReagentData t in obj)
            {
                hashcode ^= t.GetHashCode();
            }
            return hashcode;
        }
    }
}