namespace CTodo.GraphQL.GraphQLQueries;

public class GraphQLQuery
{
    public string Query { get; set; }
    public Dictionary<string, object> Variables { get; set; }
}