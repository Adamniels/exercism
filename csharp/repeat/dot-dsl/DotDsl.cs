using System;
using System.Collections;
using System.Collections.Generic;

public class Node : Element
{
    public string Value { get; }
    public Node(string value) => Value = value;

    public override bool Equals(object obj) => obj is Node node && node.Value == Value;
    public override int GetHashCode() => HashCode.Combine(Value);

}

public class Edge : Element
{
    public string From { get; }
    public string To { get; }

    public Edge(string from, string to)
    {
        From = from;
        To = to;
    }
    public override bool Equals(object obj) => obj is Edge edge && edge.From == From && edge.To == To;
    public override int GetHashCode() => HashCode.Combine(From, To);
}

public record Attr(string Key, string Value);

public class Graph : Element
{
    private readonly List<Node> nodes = new();
    private readonly List<Edge> edges = new();
    public IEnumerable<Node> Nodes => nodes;
    public IEnumerable<Edge> Edges => edges;
    public IEnumerable<Attr> Attrs => attrs;
    public void Add(Node node) => nodes.Add(node);
    public void Add(Edge edge) => edges.Add(edge);
}

public abstract class Element : IEnumerable<Attr>
{
    protected readonly List<Attr> attrs = new();

    public void Add(string key, string value) => attrs.Add(new Attr(key, value));

    public IEnumerator<Attr> GetEnumerator() => attrs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
