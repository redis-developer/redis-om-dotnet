using System.Text.Json.Serialization;
using Redis.OM.Modeling;
using Redis.OM.Modeling.Vectors;
using StackExchange.Redis;

namespace Redis.OM.Unit.Tests;

[Document(StorageType = StorageType.Json)]
public class ObjectWithVector
{
    [RedisIdField]
    public string Id { get; set; }

    [Indexed] public string Name { get; set; }

    [Indexed] public int Num { get; set; }
    
    [Vector(Algorithm = VectorAlgorithm.HNSW, Dim = 10)]
    public double[] SimpleHnswVector { get; set; }

    [Vector(Algorithm = VectorAlgorithm.FLAT)]
    [SimpleVectorizer]
    public string SimpleVectorizedVector { get; set; }

    public VectorScores VectorScores { get; set; }
}

[Document(StorageType = StorageType.Hash)]
public class ObjectWithVectorHash
{
    [RedisIdField]
    public string Id { get; set; }

    [Indexed] public string Name { get; set; }
    
    [Indexed] public int Num { get; set; }

    [Vector(Algorithm = VectorAlgorithm.HNSW, Dim = 10)]
    public double[] SimpleHnswVector { get; set; }
    
    [Vector(Algorithm = VectorAlgorithm.FLAT)]
    [SimpleVectorizer]
    public string SimpleVectorizedVector { get; set; }

    public VectorScores VectorScores { get; set; }
}

[Document(StorageType = StorageType.Json, Prefixes = new []{"Simple"})]
public class ToyVector
{
    [RedisIdField] public string Id { get; set; }
    [Vector(Dim=6)]public double[] SimpleVector { get; set; }
}