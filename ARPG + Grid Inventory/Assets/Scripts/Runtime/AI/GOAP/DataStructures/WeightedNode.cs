using System;
using UnityEngine;

[Serializable]
public class WeightedNode<T> : IWeighted {

    [SerializeField] private T     _element;
    [SerializeField] private float _weight;

    public T     Element => _element;
    public float Weight  => _weight;


    public WeightedNode(T element, float weight) {
        _element = element;
        _weight  = weight;
    }

    public override string ToString()
    {
        return $"{_element} - {_weight}";
    }
}