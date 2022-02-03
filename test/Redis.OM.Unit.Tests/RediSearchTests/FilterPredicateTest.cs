﻿using Moq;
using System;
using System.Linq;
using Redis.OM.Aggregation;
using Redis.OM.Aggregation.AggregationPredicates;
using Redis.OM.Contracts;
using Redis.OM;
using Redis.OM.Modeling;
using Xunit;

namespace Redis.OM.Unit.Tests.RediSearchTests
{
    public class FilterPredicateTest
    {
        Mock<IRedisConnection> _mock = new Mock<IRedisConnection>();
        RedisReply _mockReply = new RedisReply[]
        {
            new RedisReply(1),
            new RedisReply(new RedisReply[]
            {
                "FakeResult",
                "Blah"
            })
        };

        [Fact]
        public void TestBasicFilter()
        {
            var expectedPredicate = "5 < 6";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var five = 5;
            var six = 6;

            var res = collection.Filter(x=>five<six).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestBasicFilterString()
        {
            var expectedPredicate = "@Name == 'steve'";
            _mock.Setup(x => x.Execute(
                    "FT.AGGREGATE",It.IsAny<string[]>()
                    ))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x=>x.RecordShell.Name == "steve").ToArray();

            _mock.Verify(x=>x.Execute("FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate));
            Assert.Equal("Blah", res[0]["FakeResult"]);
        }
        
        [Fact]
        public void TestBasicFilterStringUnpackedFromVariable()
        {
            var expectedPredicate = "@Name == 'steve'";
            _mock.Setup(x => x.Execute(
                    "FT.AGGREGATE",It.IsAny<string[]>()
                ))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var steve = "steve";
            var res = collection.Filter(x=>x.RecordShell.Name == steve).ToArray();

            _mock.Verify(x=>x.Execute("FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate));
            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestBasicFilterNullableString()
        {
            var expectedPredicate = "@NullableStringField == 'steve'";
            _mock.Setup(x => x.Execute(
                    "FT.AGGREGATE",It.IsAny<string[]>()
                ))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);
            
            var res = collection.Filter(x=>x.RecordShell.NullableStringField == "steve").ToArray();

            _mock.Verify(x=>x.Execute("FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate));
            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestFilterSingleIdentifier()
        {
            var expectedPredicate = "@Age < 6";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x => x.RecordShell.Age < 6).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestFilterMathFunctions()
        {
            var expectedPredicate = "abs(@Age) < 6";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x => Math.Abs((int)x.RecordShell.Age) < 6).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestFilterGeoFunctions()
        {
            var expectedPredicate = "geodistance(@Home,@Work) < 6";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x => ApplyFunctions.GeoDistance((GeoLoc)x.RecordShell.Home, (GeoLoc)x.RecordShell.Work) < 6).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestFilterStringFunction()
        {
            var expectedPredicate = "contains(@Name,\"ste\")";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x => x.RecordShell.Name.Contains("ste")).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }

        [Fact]
        public void TestFilterDatetimeFunction()
        {
            var expectedPredicate = "dayofweek(@LastTimeOnline) < 6";
            _mock.Setup(x => x.Execute(
                "FT.AGGREGATE",
                "person-idx",
                "*",
                "FILTER",
                expectedPredicate))
                .Returns(_mockReply);
            var collection = new RedisAggregationSet<Person>(_mock.Object);

            var res = collection.Filter(x => ApplyFunctions.DayOfWeek((long)x.RecordShell.LastTimeOnline) < 6).ToArray();

            Assert.Equal("Blah", res[0]["FakeResult"]);
        }
    }
}
