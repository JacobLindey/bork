using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class LootTable : Resource
{
    [Export]
    private Array<LootTableEntry> Entries;
    
    public Resource GetNextEntry()
    {
        var totalWeight = Entries.Sum(x => x.Weight);
        var rand = GD.RandRange(0, totalWeight);
        foreach (var entry in Entries)
        {
            if (rand <= entry.Weight)
            {
                return entry.Resource;
            }

            rand -= entry.Weight;
        }

        return Entries.Last();
    }

    public T GetNextEntry<T>() where T : Resource
    {
        return (T) GetNextEntry();
    }
}