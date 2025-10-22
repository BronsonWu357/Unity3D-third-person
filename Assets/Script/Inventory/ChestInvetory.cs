using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestInvetory : MonoBehaviour
{
    [SerializeField] public List<Item> items = new List<Item>();

    [SerializeField] public List<int> indexList = new List<int>();

    [SerializeField] public List<int> countList = new List<int>();
}
