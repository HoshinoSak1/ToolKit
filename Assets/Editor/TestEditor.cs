//using UnityEngine;
//using UnityEditor;
//using Sirenix.OdinInspector.Editor;
//using System.Collections.Generic;
//using Sirenix.OdinInspector;

//public class GridMapEditor : OdinEditorWindow
//{
//    [SerializeField]
//    private List<List<GridCell>> grid = new List<List<GridCell>>();

//    private const int gridSize = 10;
//    private const float cellSize = 20f;

//    private bool isMouseDragging;
//    private GridCellType currentBrush = GridCellType.Ground;

//    [MenuItem("MyTools/Grid Map Editor")]
//    private static void OpenWindow()
//    {
//        GetWindow<GridMapEditor>().Show();
//    }

//    [Button("Generate Map")]
//    private void GenerateMap()
//    {
//        grid.Clear();
//        for (int i = 0; i < gridSize; i++)
//        {
//            List<GridCell> row = new List<GridCell>();
//            for (int j = 0; j < gridSize; j++)
//            {
//                row.Add(new GridCell());
//            }
//            grid.Add(row);
//        }
//    }

//    protected override void OnGUI()
//    {
//        base.OnGUI();

//        EditorGUILayout.Space();

//        if (grid.Count == 0 || grid[0].Count == 0)
//        {
//            EditorGUILayout.LabelField("No map generated.");
//            return;
//        }

//        EditorGUILayout.LabelField("Grid Map");

//        // ����ÿ������
//        for (int i = 0; i < grid.Count; i++)
//        {
//            EditorGUILayout.BeginHorizontal();
//            for (int j = 0; j < grid[i].Count; j++)
//            {
//                var cell = grid[i][j];
//                Rect rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(cellSize), GUILayout.Height(cellSize));
//                DrawGridCell(rect, cell.type);
//                HandleInput(rect, i, j);
//            }
//            EditorGUILayout.EndHorizontal();
//        }
//    }

//    // ���ݸ������ͻ��Ƹ���
//    private void DrawGridCell(Rect rect, GridCellType type)
//    {
//        Color color = Color.white;

//        switch (type)
//        {
//            case GridCellType.Ground:
//                color = Color.black;
//                break;
//            case GridCellType.Wall:
//                color = Color.white;
//                break;
//            case GridCellType.Water:
//                color = Color.blue;
//                break;
//            case GridCellType.Tree:
//                color = Color.green;
//                break;
//        }

//        EditorGUI.DrawRect(rect, color);
//    }

//    // �����������
//    private void HandleInput(Rect rect, int row, int col)
//    {
//        Event currentEvent = Event.current;
//        EventType eventType = currentEvent.type;

//        // ���������¼�
//        if (eventType == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
//        {
//            isMouseDragging = true;

//            // ���ݵ�ǰ��ˢ���ĸ�������
//            switch (currentBrush)
//            {
//                case GridCellType.Ground:
//                    grid[row][col].type = GridCellType.Ground;
//                    break;
//                case GridCellType.Wall:
//                    grid[row][col].type = GridCellType.Wall;
//                    break;
//                case GridCellType.Water:
//                    grid[row][col].type = GridCellType.Water;
//                    break;
//                case GridCellType.Tree:
//                    grid[row][col].type = GridCellType.Tree;
//                    break;
//            }

//            Repaint();
//        }
//        else if (eventType == EventType.MouseUp)
//        {
//            isMouseDragging = false;
//        }

//        // ���϶������г���������λ��
//        if (isMouseDragging && eventType == EventType.MouseDrag && rect.Contains(currentEvent.mousePosition))
//        {
//            // ���ݵ�ǰ��ˢ���ĸ�������
//            switch (currentBrush)
//            {
//                case GridCellType.Ground:
//                    grid[row][col].type = GridCellType.Ground;
//                    break;
//                case GridCellType.Wall:
//                    grid[row][col].type = GridCellType.Wall;
//                    break;
//                case GridCellType.Water:
//                    grid[row][col].type = GridCellType.Water;
//                    break;
//                case GridCellType.Tree:
//                    grid[row][col].type = GridCellType.Tree;
//                    break;
//            }

//            Repaint();
//        }
//    }

//    // ����������
//    [OnInspectorGUI]
//    protected void OnInspectorGUI()
//    {
//        GUILayout.Space(10);

//        GUILayout.Label("Brush:");
//        currentBrush = (GridCellType)EditorGUILayout.EnumPopup(currentBrush);
//    }
//}

//public class GridCell
//{
//    public GridCellType type;
//}

//public enum GridCellType
//{
//    Ground,
//    Wall,
//    Water,
//    Tree
//}
