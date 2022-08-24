﻿using System;
using Redis.OM.Modeling;

namespace Redis.OM.Unit.Tests
{
    [Document(StorageType = StorageType.Json)]
    public class JsonPerson
    {
        [RedisIdField] public Ulid Id { get; set; }
        [Indexed] public string Name { get; set; }
    }

    [Document(StorageType = StorageType.Hash)]
    public class HashPerson
    {
        [RedisIdField] public Ulid Id { get; set; }
        [Indexed] public string Name { get; set; }
    }
}