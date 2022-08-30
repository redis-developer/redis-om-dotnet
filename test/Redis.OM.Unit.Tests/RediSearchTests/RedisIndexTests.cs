﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Redis.OM.Modeling;
using Xunit;

namespace Redis.OM.Unit.Tests.RediSearchTests
{
    public class RedisIndexTests
    {

        [Document(IndexName = "TestPersonClassHappyPath-idx", StorageType = StorageType.Hash)]
        public class TestPersonClassHappyPath
        {
            [Searchable(Sortable = true)]
            public string Name { get; set; }
            [Indexed(Sortable = true)]
            public int Age { get; set; }
            public double Height { get; set; }
            public string[] NickNames { get; set; }
        }

        [Document(IndexName = "TestPersonClassHappyPath-idx", StorageType = StorageType.Hash, Prefixes = new []{"Person:"})]
        public class TestPersonClassOverridenPrefix
        {
            [Searchable(Sortable = true)]
            public string Name { get; set; }
            [Indexed(Sortable = true)]
            public int Age { get; set; }
            public double Height { get; set; }
            public string[] NickNames { get; set; }
        }

        [Fact]
        public void TestIndexSerializationHappyPath()
        {
            var expected = new[] { "TestPersonClassHappyPath-idx",
                "ON", "Hash", "PREFIX", "1", "Redis.OM.Unit.Tests.RediSearchTests.RedisIndexTests+TestPersonClassHappyPath:", "SCHEMA",
                "Name", "TEXT", "SORTABLE", "Age", "NUMERIC", "SORTABLE" };
            var indexArr = typeof(TestPersonClassHappyPath).SerializeIndex();

            Assert.True(expected.SequenceEqual(indexArr));
        }

        [Fact]
        public void TestIndexSerializationOverridenPrefix()
        {
            var expected = new[] { "TestPersonClassHappyPath-idx",
                "ON", "Hash", "PREFIX", "1", "Person:", "SCHEMA",
                "Name", "TEXT", "SORTABLE", "Age", "NUMERIC", "SORTABLE" };
            var indexArr = typeof(TestPersonClassOverridenPrefix).SerializeIndex();

            Assert.True(expected.SequenceEqual(indexArr));
        }

        [Fact]
        public void TestCreateIndex()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.DropIndex(typeof(TestPersonClassHappyPath));
            var res = connection.CreateIndex(typeof(TestPersonClassHappyPath));
            Assert.True(res);
            connection.DropIndex(typeof(TestPersonClassHappyPath));
        }

        [Fact]
        public async Task TestCreateIndexAsync()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            var res = await connection.CreateIndexAsync(typeof(TestPersonClassHappyPath));
            Assert.True(res);
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
        }

        [Fact]
        public void TestCreateAlreadyExistingIndex()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.CreateIndex(typeof(TestPersonClassHappyPath));
            var res = connection.CreateIndex(typeof(TestPersonClassHappyPath));
            Assert.False(res);
            connection.DropIndex(typeof(TestPersonClassHappyPath));
        }

        [Fact]
        public void TestDropExistingIndex()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.DropIndex(typeof(TestPersonClassHappyPath));
            connection.CreateIndex(typeof(TestPersonClassHappyPath));
            var res = connection.DropIndex(typeof(TestPersonClassHappyPath));
            Assert.True(res);
        }

        [Fact]
        public async Task TestDropExistingIndexAsync()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            await connection.CreateIndexAsync(typeof(TestPersonClassHappyPath));
            var res = await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            Assert.True(res);
        }

        [Fact]
        public void TestDropIndexWhichDoesNotExist()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.DropIndex(typeof(TestPersonClassHappyPath));
            var res = connection.DropIndex(typeof(TestPersonClassHappyPath));
            Assert.False(res);
        }

        [Fact]
        public async Task TestDropIndexWhichDoesNotExistAsync()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            var res = await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            Assert.False(res);
        }

        [Fact]
        public void TestGetIndexInfoHappyPath()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.DropIndex(typeof(TestPersonClassHappyPath));
            connection.CreateIndex(typeof(TestPersonClassHappyPath));
            var indexInfo = connection.GetIndexInfo(typeof(TestPersonClassHappyPath));
            Assert.NotNull(indexInfo);
            Assert.True(indexInfo.IndexName == "TestPersonClassHappyPath-idx");
            Assert.True(indexInfo.IndexDefinition?.Identifier == "HASH");
            Assert.True(indexInfo.IndexDefinition?.Prefixes?[0] == "Redis.OM.Unit.Tests.RediSearchTests.RedisIndexTests+TestPersonClassHappyPath:");
            Assert.NotNull(indexInfo.Indexing);
            var attributes = indexInfo.Attributes.ToList();
            Assert.Contains(attributes, x => x.Attribute == "Name" && x.Sortable == true);
            Assert.Contains(attributes, x => x.Attribute == "Age" && x.Sortable == true);
            Assert.DoesNotContain(attributes, x => x.Attribute == "Height");
            connection.DropIndex(typeof(TestPersonClassHappyPath));
        }

        [Fact]
        public async Task TestGetIndexInfoHappyPathAsync()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            await connection.CreateIndexAsync(typeof(TestPersonClassHappyPath));
            var indexInfo = await connection.GetIndexInfoAsync(typeof(TestPersonClassHappyPath));
            Assert.NotNull(indexInfo);
            Assert.True(indexInfo.IndexName == "TestPersonClassHappyPath-idx");
            Assert.True(indexInfo.IndexDefinition?.Identifier == "HASH");
            Assert.True(indexInfo.IndexDefinition?.Prefixes?[0] == "Redis.OM.Unit.Tests.RediSearchTests.RedisIndexTests+TestPersonClassHappyPath:");
            Assert.NotNull(indexInfo.Indexing);
            var attributes = indexInfo.Attributes.ToList();
            Assert.Contains(attributes, x => x.Attribute == "Name" && x.Sortable == true);
            Assert.Contains(attributes, x => x.Attribute == "Age" && x.Sortable == true);
            Assert.DoesNotContain(attributes, x => x.Attribute == "Height");
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
        }

        [Fact]
        public void TestGetIndexInfoWhichDoesNotExist()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            connection.DropIndex(typeof(TestPersonClassHappyPath));
            var indexInfo = connection.GetIndexInfo(typeof(TestPersonClassHappyPath));
            Assert.Null(indexInfo);
        }

        [Fact]
        public async Task TestGetIndexInfoWhichDoesNotExistAsync()
        {
            var host = Environment.GetEnvironmentVariable("STANDALONE_HOST_PORT") ?? "localhost";
            var provider = new RedisConnectionProvider($"redis://{host}");
            var connection = provider.Connection;
            await connection.DropIndexAsync(typeof(TestPersonClassHappyPath));
            var indexInfo = await connection.GetIndexInfoAsync(typeof(TestPersonClassHappyPath));
            Assert.Null(indexInfo);
        }
    }
}
