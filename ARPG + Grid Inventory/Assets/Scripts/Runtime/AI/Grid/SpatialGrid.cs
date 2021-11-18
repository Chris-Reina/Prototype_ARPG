using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//IA2-P2 
//Editamos la clase para que pueda utilizarse en 3 dimensiones, voy a marcar usos con el mismo tag ("//IA2-P2")
public class SpatialGrid : MonoBehaviour 
{
    public static SpatialGrid StaticGrid { get; private set; }

    #region Variables
    //Width == X
    //Depth == Z
    //Height == Y

    public float x;
    public float y;
    public float z;

    public float cellWidth;
    public float cellDepth;
    public float cellHeight;

    public int width;
    public int depth;
    public int height;

    private Dictionary<IGridEntity, Tuple<int, int, int>> _lastPositions = new Dictionary<IGridEntity, Tuple<int, int, int>>();

    private HashSet<IGridEntity>[,,] _buckets;

    readonly public Tuple<int,int,int> Outside = Tuple.Create(-1, -1, -1);
    readonly public IGridEntity[] Empty = new IGridEntity[0];

    #endregion

    #region Funciones

    private void Awake()
    {
        if (StaticGrid == null)
            StaticGrid = this;
        else if(StaticGrid != this)
            Destroy(this);

        _buckets       = new HashSet<IGridEntity>[width, height, depth];

        //creamos todos los hashsets
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                for (var k = 0; k < depth; k++)
                {
                    _buckets[i, j, k] = new HashSet<IGridEntity>();

                }
            }
        }
        
        var ents = RecursiveWalker(transform)
                  .Select(n => n.GetComponent<IGridEntity>())
                  .Where(n => n != null);

        foreach (var e in ents) {
            e.OnMove += UpdateEntity;
            UpdateEntity(e);
        }

        var pc = FindObjectOfType<PlayerController>();
        pc.OnMove += UpdateEntity;
        UpdateEntity(pc);
    }

    public void UpdateEntity(IGridEntity entity) {
        var lastPos    = _lastPositions.ContainsKey(entity) ? _lastPositions[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.Position);

        //Misma posición, no necesito hacer nada
        if (lastPos.Equals(currentPos))
            return;

        //Lo "sacamos" de la posición anterior
        if (IsInsideGrid(lastPos))
        {
            _buckets[lastPos.Item1, lastPos.Item2, lastPos.Item3].Remove(entity);
        }

        //Lo "metemos" a la celda nueva, o lo sacamos si salio de la grilla
        if (IsInsideGrid(currentPos))
        {
            _buckets[currentPos.Item1, currentPos.Item2, currentPos.Item3].Add(entity);
            _lastPositions[entity] = currentPos;
        }
        else
            _lastPositions.Remove(entity);
    }

    public IEnumerable<IGridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition) {
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), Mathf.Min(aabbFrom.y, aabbTo.y), Mathf.Min(aabbFrom.z, aabbTo.z));
        var to   = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), Mathf.Max(aabbFrom.y, aabbTo.y), Mathf.Max(aabbFrom.z, aabbTo.z));

        var fromCoord = GetPositionInGrid(from);
        var toCoord   = GetPositionInGrid(to);

        fromCoord = Tuple.Create(
                        Util.Clamp(fromCoord.Item1, 0, width), 
                        Util.Clamp(fromCoord.Item2, 0, height), 
                        Util.Clamp(fromCoord.Item3, 0, depth));
        toCoord   = Tuple.Create(
                        Util.Clamp(toCoord.Item1,   0, width), 
                        Util.Clamp(toCoord.Item2,   0, height), 
                        Util.Clamp(toCoord.Item3,   0, depth));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;

        // Creamos tuplas de cada celda
        var cols = Util.Generate(fromCoord.Item1, x => x + 1)
                       .TakeWhile(n => n < width && n <= toCoord.Item1);

        var rows = Util.Generate(fromCoord.Item2, y => y + 1)
                       .TakeWhile(y => y < height && y <= toCoord.Item2);
        
        var stacks = Util.Generate(fromCoord.Item3, z => z + 1)
                       .TakeWhile(z => z < depth && z <= toCoord.Item3);

        var cells = cols.SelectMany(
                                    col => rows.SelectMany(
                                        row => stacks.Select(
                                            stack => Tuple.Create(col, row, stack)))
                                   );

        // Iteramos las que queden dentro del criterio
        return cells
              .SelectMany(cell => _buckets[cell.Item1, cell.Item2, cell.Item3])
              .Where(e =>
                         from.x <= e.Position.x && e.Position.x <= to.x &&
                         from.y <= e.Position.y && e.Position.y <= to.y &&
                         from.z <= e.Position.z && e.Position.z <= to.z
                    )
              .Where(n => filterByPosition(n.Position));
    }

    public Tuple<int,int,int> GetPositionInGrid(Vector3 pos) {
        //quita la diferencia, divide segun las celdas y floorea
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.y - y) / cellHeight),
                            Mathf.FloorToInt((pos.z - z) / cellDepth));
    }

    public bool IsInsideGrid(Tuple<int,int,int> position) {
        //si es menor a 0 o mayor a width o height, no esta dentro de la grilla
        return 0 <= position.Item1 && position.Item1 < width &&
               0 <= position.Item2 && position.Item2 < height &&
               0 <= position.Item3 && position.Item3 < depth;
    }

    void OnDestroy() {
        var ents = RecursiveWalker(transform).Select(n => n.GetComponent<IGridEntity>())
                                             .Where(n => n != null);
        
        foreach (var e in ents) e.OnMove -= UpdateEntity;
    }

    #region GENERATORS

    private static IEnumerable<Transform> RecursiveWalker(Transform parent) {
        foreach (Transform child in parent) {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }

    #endregion

    #endregion

    #region GRAPHIC REPRESENTATION

    public bool areGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;

    private void OnDrawGizmosSelected() {
        var rows = Util.Generate(z, curr => curr + cellDepth)
                       .Select(row => Tuple.Create(new Vector3(x,                     0, row),
                                                   new Vector3(x + cellWidth * width, 0, row)));

        //equivalente de rows
        /*for (int i = 0; i <= height; i++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, z + cellHeight * i), new Vector3(x + cellWidth * width,0, z + cellHeight * i));
        }*/

        var cols = Util.Generate(x, curr => curr + cellWidth)
                       .Select(col => Tuple.Create(new Vector3(col, 0, z),
                                                   new Vector3(col, 0, z + cellDepth * depth)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(depth + 1));

        foreach (var elem in allLines) {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }

        if (_buckets == null || areGizmosShutDown) return;

        var originalCol = GUI.color;
        GUI.color = Color.red;
        if (!activatedGrid) {
            var allElems = new List<IGridEntity>();
            foreach (var elem in _buckets)
                allElems = allElems.Concat(elem).ToList();

            int connections = 0;
            foreach (var entity in allElems) {
                foreach (var neighbour in allElems.Where(x => x != entity)) {
                    Gizmos.DrawLine(entity.Position, neighbour.Position);
                    connections++;
                }

                if (showLogs)
                    Debug.Log("tengo " + connections + " conexiones por individuo");
                connections = 0;
            }
        }
        else {
            int connections = 0;
            foreach (var elem in _buckets) {
                foreach (var ent in elem) {
                    foreach (var n in elem.Where(x => x != ent)) {
                        Gizmos.DrawLine(ent.Position, n.Position);
                        connections++;
                    }

                    if (showLogs)
                        Debug.Log("tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
        }

        GUI.color = originalCol;
        showLogs  = false;
    }

    #endregion
}
