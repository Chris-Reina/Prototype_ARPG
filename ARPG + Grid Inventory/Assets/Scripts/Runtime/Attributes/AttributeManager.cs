using System.Collections.Generic;
using UnityEngine;
using DoaT.Attributes;

public class AttributeManager : MonoBehaviour
{
    [Tooltip("List of attributes managed by this manager.")]
    [SerializeField] public List<Attribute> m_Attributes = new List<Attribute> { new Attribute("Health", 100) };

    //public List<Attribute> Attributes { get { return m_Attributes; } }
    public Dictionary<string,Attribute> NameToAttributeMap { get { return m_NameToAttributeMap; } }
    
    private Dictionary<string, Attribute> m_NameToAttributeMap = new Dictionary<string, Attribute>();

    private void Awake()
    {
        RecalculateDictionary();
    }

    private void Update()
    {
        //DebugTool();
    }

    /// <summary>
    /// Creates a new Attribute.
    /// </summary>
    /// <param name="name">Name of the Attribute.</param>
    public void AddAttribute(string name)
    {
        if (string.IsNullOrEmpty(name))        
            return;        

        if (!IsNameUnique(name))
        {
            name = AddSuffixToName(name);
        }

        m_Attributes.Add(new Attribute(name));
        m_NameToAttributeMap.Add(name, m_Attributes[m_Attributes.Count - 1]);
    }

    /// <summary>
    /// Removes the attribute in the given index, if any.
    /// </summary>
    /// <param name="index">Indexof the Attribute you want to remove.</param>
    public void RemoveAttribute(int index)
    {
        if(index < m_Attributes.Count)
        {
            m_NameToAttributeMap.Remove(m_Attributes[index].Name);
            m_Attributes.RemoveAt(index);
        }
    }

    /// <summary>
    /// Tries to return the Attribute mapped to a given name. Can be null.
    /// </summary>
    /// <param name="name">Name of the Attribute you want to get.</param>
    /// <returns></returns>
    public Attribute TryGetAttribute(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;        

        if (m_NameToAttributeMap.ContainsKey(name))
            return m_NameToAttributeMap[name];

        return null;
    }

    /// <summary>
    /// Tries to rename an Attribute. Returns a boolean based on success.
    /// </summary>
    /// <param name="oldName">Old name of the Attribute.</param>
    /// <param name="newName">New name of the Attribute.</param>
    /// <returns></returns>
    public bool RenameAttribute(string oldName, string newName)
    {
        if (string.IsNullOrEmpty(newName))
            return false;

        if (!IsNameUnique(newName))
        {
            newName = AddSuffixToName(newName);
            Debug.Log(newName);
        }

        var temp = TryGetAttribute(oldName);
        if(temp != null)
        {
            temp.Rename(newName);
            m_NameToAttributeMap.Remove(oldName);
            m_NameToAttributeMap.Add(newName, temp);
            return true;
        }

        return false;
    }


    #region Utility
    public bool IsNameUnique(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        
        if (m_Attributes.Count == 0)        
            return true;        

        for (int i = 0; i < m_Attributes.Count; ++i)
        {
            if (m_Attributes[i] != null && m_Attributes[i].Name == name)
            {
                return false;
            }
        }

        return true;
    }
    public string AddSuffixToName(string name)
    {
        var suffixIndex = 1;
        while (!IsNameUnique(name + " (" + suffixIndex + ")"))
        {
            suffixIndex++;
        }

        return name += " (" + suffixIndex + ")";
    }
    public void RecalculateDictionary()
    {
        m_NameToAttributeMap.Clear();

        for (int i = 0; i < m_Attributes.Count; i++)
        {
            m_NameToAttributeMap.Add(m_Attributes[i].Name, m_Attributes[i]);
        }
    }
    #endregion

    #region Debug
    private void DebugTool()
    {
        Debug.Log("Hay " + m_Attributes.Count + " atributos en la lista.");

        for (int i = 0; i < m_Attributes.Count; i++)
        {
            Debug.Log("Soy el attribute " + m_Attributes[i].Name + " y estoy en el indice " + i);
        }
    }
    #endregion
}
