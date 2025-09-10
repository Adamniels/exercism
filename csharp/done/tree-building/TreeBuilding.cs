public class TreeBuildingRecord
{
    private const int RootRecordId = 0;

    public int ParentId { get; set; }
    public int RecordId { get; set; }

    public bool IsRoot => RecordId == RootRecordId;
}

public class Tree
{
    public Tree(int id, int parentId)
    {
        Id = id;
        ParentId = parentId;
        Children = new List<Tree>();
    }

    public int Id { get; }

    public int ParentId { get; }

    public List<Tree> Children { get; }

    public bool IsLeaf => Children.Count == 0;
}

public static class TreeBuilder
{
    private const int RootRecordId = 0;

    public static Tree BuildTree(IEnumerable<TreeBuildingRecord> records)
    {
        var orderedRecords = GetOrderedRecords(records);

        if (orderedRecords.Count == 0)
            throw new ArgumentException();

        var nodes = new Dictionary<int, Tree>();
        var previousRecordId = -1;

        foreach (var record in orderedRecords)
        {
            ValidateRecord(record, previousRecordId);

            nodes[record.RecordId] = new Tree(record.RecordId, record.ParentId);

            if (!record.IsRoot)
                nodes[record.ParentId].Children.Add(nodes[record.RecordId]);

            previousRecordId++;
        }

        return nodes[RootRecordId];
    }

    private static void ValidateRecord(TreeBuildingRecord record, int previousRecordId)
    {
        if (record.IsRoot && record.ParentId != RootRecordId)
            throw new ArgumentException();
        else if (!record.IsRoot && record.ParentId >= record.RecordId)
            throw new ArgumentException();
        else if (!record.IsRoot && record.RecordId != previousRecordId + 1)
            throw new ArgumentException();
    }

    private static List<TreeBuildingRecord> GetOrderedRecords(IEnumerable<TreeBuildingRecord> records)
    {
        return records.OrderBy(record => record.RecordId).ToList();
    }
}

// public class TreeBuildingRecord
// {
//     public int ParentId { get; set; }
//     public int RecordId { get; set; }
// }
//
// public class Tree
// {
//     public int Id { get; set; }
//     public int ParentId { get; set; }
//
//     public List<Tree> Children { get; set; }
//
//     public bool IsLeaf => Children.Count == 0;
// }
//
// public static class TreeBuilder
// {
//
//     public static Tree BuildTree(IEnumerable<TreeBuildingRecord> records)
//     {
//         var ordered = new SortedList<int, TreeBuildingRecord>();
//
//         foreach (var record in records)
//         {
//             ordered.Add(record.RecordId, record); // lägger till i den sorterade listan , verkar sortera på RecordId
//         }
//
//         records = ordered.Values;
//
//         var trees = new List<Tree>();
//         var previousRecordId = -1;
//
//         foreach (var record in records) // går igenom den sorterade listan
//         {
//             var t = new Tree { Children = new List<Tree>(), Id = record.RecordId, ParentId = record.ParentId };
//             trees.Add(t);
//
//             if ((t.Id == 0 && t.ParentId != 0) ||
//                 (t.Id != 0 && t.ParentId >= t.Id) ||
//                 (t.Id != 0 && t.Id != previousRecordId + 1))
//             {
//                 throw new ArgumentException();
//             }
//
//             ++previousRecordId;
//         }
//
//         if (trees.Count == 0)
//         {
//             throw new ArgumentException();
//         }
//
//         for (int i = 1; i < trees.Count; i++)
//         {
//             var t = trees.First(x => x.Id == i);
//             var parent = trees.First(x => x.Id == t.ParentId);
//             parent.Children.Add(t);
//         }
//
//         var r = trees.First(t => t.Id == 0);
//         return r;
//     }
// }
