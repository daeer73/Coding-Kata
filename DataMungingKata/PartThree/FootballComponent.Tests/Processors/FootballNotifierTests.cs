﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DataMungingCore.Interfaces;
using DataMungingCore.Types;
using FluentAssertions;
using FootballComponent.Processors;
using FootballComponent.Types;
using NSubstitute;
using Serilog;
using Xunit;

namespace FootballComponent.Tests.Processors
{
    public class FootballNotifierTests
    {
        private readonly ILogger _logger;
        private readonly FootballNotifier _footballNotifier;

        public FootballNotifierTests()
        {
            _logger = Substitute.For<ILogger>();
            _footballNotifier = new FootballNotifier(_logger);
        }


        [Fact]
        public async Task Test_get_team_with_empty_list_throws_exception()
        {
            // Arrange.
            IList<IDataType> data = new List<IDataType>();

            // Act.
            // Assert.
            await Assert.ThrowsAsync<ArgumentException>(() => _footballNotifier.NotifyAsync(data)).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetInValidFootballData))]
        public async Task Test_get_team_with_negative_points_throws_exception(IList<IDataType> data)
        {
            // Arrange.
            // Act.
            // Assert.
            await Assert.ThrowsAsync<ArgumentException>(() => _footballNotifier.NotifyAsync(data)).ConfigureAwait(true);
        }

        [Fact]
        public async Task Test_get_team_with_null_list_throws_null_exception()
        {
            // Arrange.
            // Act.
            // Assert.
            await Assert.ThrowsAsync<ArgumentNullException>(() => _footballNotifier.NotifyAsync(null)).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetValidFootballData))]
        public async Task Test_get_team_with_valid_list_returns_expected(string expectedTeam, IList<IDataType> data)
        {
            // Arrange.
            // Act.
            var actualTeam = await _footballNotifier.NotifyAsync(data).ConfigureAwait(true);

            // Assert.
            actualTeam.ProcessResult.Should().Be(expectedTeam);
        }

        #region Test Data.

        public static IEnumerable<object[]> GetValidFootballData
        {
            get
            {
                yield return new object[]
                {
                    "Arsenal",
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football {TeamName = "Arsenal", AgainstPoints = 22, ForPoints = 22}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Bournemouth", AgainstPoints = 23, ForPoints = 21}}
                    }
                };
                yield return new object[]
                {
                    "Bournemouth",
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football {TeamName = "Arsenal", AgainstPoints = 42, ForPoints = 22}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Bournemouth", AgainstPoints = 5, ForPoints = 6}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Manchester_U", AgainstPoints = 65, ForPoints = 1}}
                    }
                };
                yield return new object[]
                {
                    "Bournemouth",
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football {TeamName = "Arsenal", AgainstPoints = 42, ForPoints = 22}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Bournemouth", AgainstPoints = 5, ForPoints = 6}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Manchester_U", AgainstPoints = 65, ForPoints = 1}},
                        new ContainingDataType
                            {Data = new Football {TeamName = "Aston Villa", AgainstPoints = 9, ForPoints = 3}}
                    }
                };
            }
        }

        public static IEnumerable<object[]> GetInValidFootballData
        {
            get
            {
                yield return new object[]
                {
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football { TeamName = "", AgainstPoints = 12, ForPoints = 25}}
                    }
                };
                yield return new object[]
                {
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football { TeamName = "hello", AgainstPoints = -1, ForPoints = 25}}
                    }
                };
                yield return new object[]
                {
                    new List<IDataType>
                    {
                        new ContainingDataType
                            {Data = new Football { TeamName = "hello", AgainstPoints = 31, ForPoints = -25}}
                    }
                };
            }
        }

        #endregion Test Data.
    }
}
