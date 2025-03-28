using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using TMPro.EditorUtilities;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<Item> _item;
    public List<Item> _currentItems = new List<Item>();

    [SerializeField] private GameObject _fakeItemBox;
    [SerializeField] private KartControler _kartControler;

    public void GetItem()
    {
        if (_currentItems.Count != 2)
        {
            int index = Random.Range(0, _item.Count);
            _currentItems.Add(_item[index]);
        }
    }
    public void UseItem()
    {
        if (_currentItems.Count > 0)
        {
            Item usedItem = _currentItems[0];

            switch (usedItem.itemType)
            {
                case Type.Boost:
                    _kartControler.Boost();
                    break;
                case Type.LeftDash:
                    _kartControler.Dash(-1);
                    break;
                case Type.RightDash:
                    _kartControler.Dash(1);
                    break;
                case Type.Trap:
                    Instantiate(_fakeItemBox,transform.position, Quaternion.identity);
                    break;
            }

            usedItem.itemUseCount--;
            if (usedItem.itemUseCount <= 0)
            {
                _currentItems.RemoveAt(0);
                _currentItems[0] = _currentItems[1];
                _currentItems[1] = null;
            }
        }
    }
}
